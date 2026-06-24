using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Utilities;
using myShop.Entities.Models;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;
using Stripe.Checkout;

namespace myShop.web.Areas.Flight.Controllers
{
    [Area("Flight")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HostFlightVM HostFlightVM { get; set; }
        public int TotalCarts { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CartController
        public ActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HostFlightVM = new HostFlightVM()
            {
                CartsList = _unitOfWork.HostFlight.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "FlightProduct")

            };
            foreach (var item in HostFlightVM.CartsList)
            {
                HostFlightVM.TotalCarts += (item.Count * item.FlightProduct.TicketPrice);
            }

            return View(HostFlightVM);
        }




[HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HostFlightVM = new HostFlightVM()
            {
                CartsList = _unitOfWork.HostFlight.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "FlightProduct"),
                OrderHeader = new()
            };

            HostFlightVM.OrderHeader.ApplicationUsers = _unitOfWork.applicationUsers.GetFirstorDefault(x => x.Id == claim.Value);

            HostFlightVM.OrderHeader.Name = HostFlightVM.OrderHeader.ApplicationUsers.Name;
            HostFlightVM.OrderHeader.Address = HostFlightVM.OrderHeader.ApplicationUsers.Address;
            HostFlightVM.OrderHeader.City = HostFlightVM.OrderHeader.ApplicationUsers.City;
            HostFlightVM.OrderHeader.PhoneNumber = HostFlightVM.OrderHeader.ApplicationUsers.PhoneNumber;

            foreach (var item in HostFlightVM.CartsList)
            {
                HostFlightVM.OrderHeader.TotalPrice += (item.Count * item.FlightProduct.TicketPrice);
            }

            return View(HostFlightVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult POSTSummary(HostFlightVM ShoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.CartsList = _unitOfWork.HostFlight.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "FlightProduct");


            ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.FlightProduct.TicketPrice);
            }

            _unitOfWork.orderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Complete();

            foreach (var item in ShoppingCartVM.CartsList)
            {
                OrderDetailFlight orderDetailFlight = new OrderDetailFlight()
                {
                    FlightProductId = item.FlightProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.FlightProduct.TicketPrice,
                    Count = item.Count
                };

                _unitOfWork.orderDetailFlight.Add(orderDetailFlight);
                _unitOfWork.Complete();
            }

            var domain = "http://localhost:5054/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.FlightProduct.TicketPrice * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.FlightProduct.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;

            _unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.CartsList);
            //         _unitOfWork.Complete();
            //         return RedirectToAction("Index","Home");

        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeader.GetFirstorDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeader.UpdateStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();
            }
            List<HostFlight> shoppingcarts = _unitOfWork.HostFlight.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            HttpContext.Session.Clear();
            _unitOfWork.HostFlight.RemoveRange(shoppingcarts);
            _unitOfWork.Complete();
            return View(id);
        }



        public IActionResult Plus(int cartid)
        {

            var hostflight = _unitOfWork.HostFlight.GetFirstorDefault(x => x.Id == cartid);
            _unitOfWork.HostFlight.IncreaseCount(hostflight, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int cartid)
        {

            var hostflight = _unitOfWork.HostFlight.GetFirstorDefault(x => x.Id == cartid);

            if (hostflight.Count <= 1)
            {
                _unitOfWork.HostFlight.Remove(hostflight);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork.HostFlight.DecreaseCount(hostflight, 1);

            }
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int cartid)
        {

            var hostflight = _unitOfWork.HostFlight.GetFirstorDefault(x => x.Id == cartid);
            _unitOfWork.HostFlight.Remove(hostflight);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

    }
}
