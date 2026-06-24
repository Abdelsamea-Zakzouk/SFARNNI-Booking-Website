using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;

namespace myShop.Web.Areas.Flight.Controllers
{
    [Area("Flight")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: HomeController
        public ActionResult Index(string searchTerm, string locationFrom, string locationTo, decimal? minTicketPrice, decimal? maxTicketPrice)
        {
            // Fetch all flights
            var flights = _unitOfWork.flightProduct.GetAll();

            // Initialize the search results as null
            ViewBag.SearchResults = null;

            // Apply search filters if any criteria are provided
            if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(locationFrom) ||
                !string.IsNullOrEmpty(locationTo) || minTicketPrice.HasValue || maxTicketPrice.HasValue)
            {
                var filteredFlights = flights.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    filteredFlights = filteredFlights.Where(f => f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(locationFrom))
                {
                    filteredFlights = filteredFlights.Where(f => f.LocationFrom.Contains(locationFrom, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(locationTo))
                {
                    filteredFlights = filteredFlights.Where(f => f.LocationTo.Contains(locationTo, StringComparison.OrdinalIgnoreCase));
                }

                if (minTicketPrice.HasValue)
                {
                    filteredFlights = filteredFlights.Where(f => f.TicketPrice >= minTicketPrice.Value);
                }

                if (maxTicketPrice.HasValue)
                {
                    filteredFlights = filteredFlights.Where(f => f.TicketPrice <= maxTicketPrice.Value);
                }

                ViewBag.SearchResults = filteredFlights.ToList();
            }

            // Return all flights to the view
            return View(flights);
        }


        public ActionResult Details(int FlightProductId)
        {
            HostFlight obj = new HostFlight()
            {
                FlightProductId = FlightProductId,
                FlightProduct = _unitOfWork.flightProduct.GetFirstorDefault(x => x.Id == FlightProductId, IncludeWord: "Category"),
                Count = 1
            };

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Details(HostFlight hostFlight)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            hostFlight.ApplicationUserId = claim.Value;

            HostFlight Cartobj = _unitOfWork.HostFlight.GetFirstorDefault(
                            u => u.ApplicationUserId == claim.Value && u.FlightProductId == hostFlight.FlightProductId);

            if (Cartobj == null)
            {
                _unitOfWork.HostFlight.Add(hostFlight);
            }
            else
            {
                _unitOfWork.HostFlight.IncreaseCount(Cartobj, hostFlight.Count);
            }
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
    }
}
