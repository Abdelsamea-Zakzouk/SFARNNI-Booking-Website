using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Utilities;
using myShop.Entities.Models;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace myShop.web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HostHotelVM HostHotelVM { get; set; }
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

            HostHotelVM = new HostHotelVM()
            {
                CartsList = _unitOfWork.HostHotel.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "Product")

            };
            foreach (var item in HostHotelVM.CartsList)
            {
                HostHotelVM.TotalCarts += (item.Count * item.Product.PricePerNight);
            }

            return View(HostHotelVM);
        }


 [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HostHotelVM = new HostHotelVM()
            {
                CartsList = _unitOfWork.HostHotel.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "Product"),
                OrderHeader = new()
            };

            HostHotelVM.OrderHeader.ApplicationUsers = _unitOfWork.applicationUsers.GetFirstorDefault(x => x.Id == claim.Value);

            HostHotelVM.OrderHeader.Name = HostHotelVM.OrderHeader.ApplicationUsers.Name;
            HostHotelVM.OrderHeader.Address = HostHotelVM.OrderHeader.ApplicationUsers.Address;
            HostHotelVM.OrderHeader.City = HostHotelVM.OrderHeader.ApplicationUsers.City;
            HostHotelVM.OrderHeader.PhoneNumber = HostHotelVM.OrderHeader.ApplicationUsers.PhoneNumber;

            foreach (var item in HostHotelVM.CartsList)
            {
                HostHotelVM.OrderHeader.TotalPrice += (item.Count * item.Product.PricePerNight);
            }

            return View(HostHotelVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult POSTSummary(HostHotelVM HostHotelVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HostHotelVM.CartsList = _unitOfWork.HostHotel.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "Product");


            HostHotelVM.OrderHeader.OrderStatus = SD.Pending;
            HostHotelVM.OrderHeader.PaymentStatus = SD.Pending;
            HostHotelVM.OrderHeader.OrderDate = DateTime.Now;
            HostHotelVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var item in HostHotelVM.CartsList)
            {
                HostHotelVM.OrderHeader.TotalPrice += (item.Count * item.Product.PricePerNight);
            }

            _unitOfWork.orderHeader.Add(HostHotelVM.OrderHeader);
            _unitOfWork.Complete();

            foreach (var item in HostHotelVM.CartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = HostHotelVM.OrderHeader.Id,
                    Price = item.Product.PricePerNight,
                    Count = item.Count
                };

                _unitOfWork.orderDetailHotel.Add(orderDetail);
                _unitOfWork.Complete();
            }

            var domain = "http://localhost:5054/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={HostHotelVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in HostHotelVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.PricePerNight * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);
            HostHotelVM.OrderHeader.SessionId = session.Id;

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
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeader.UpdateStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();
            }
            List<HostHotel> shoppingcarts = _unitOfWork.HostHotel.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            HttpContext.Session.Clear();
            _unitOfWork.HostHotel.RemoveRange(shoppingcarts);
            _unitOfWork.Complete();
            return View(id);
        }



        public IActionResult Plus(int cartid)
        {

            var hosthotel = _unitOfWork.HostHotel.GetFirstorDefault(x => x.Id == cartid);
            _unitOfWork.HostHotel.IncreaseCount(hosthotel, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int cartid)
        {

            var hosthotel = _unitOfWork.HostHotel.GetFirstorDefault(x => x.Id == cartid);

            if (hosthotel.Count <= 1)
            {
                _unitOfWork.HostHotel.Remove(hosthotel);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork.HostHotel.DecreaseCount(hosthotel, 1);

            }
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int cartid)
        {

            var hosthotel = _unitOfWork.HostHotel.GetFirstorDefault(x => x.Id == cartid);
            _unitOfWork.HostHotel.Remove(hosthotel);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

    }
}
