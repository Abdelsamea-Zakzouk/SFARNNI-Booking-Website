using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;

namespace myShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: HomeController
        public ActionResult Index(string searchTerm, string location, decimal? minPrice, decimal? maxPrice)
        {
            // Fetch all products
            var products = _unitOfWork.product.GetAll();

            // Initialize the search results as null
            ViewBag.SearchResults = null;

            // Apply search filters if any criteria are provided
            if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(location) || minPrice.HasValue || maxPrice.HasValue)
            {
                var filteredProducts = products.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    filteredProducts = filteredProducts.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(location))
                {
                    filteredProducts = filteredProducts.Where(p => p.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
                }

                if (minPrice.HasValue)
                {
                    filteredProducts = filteredProducts.Where(p => p.PricePerNight >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    filteredProducts = filteredProducts.Where(p => p.PricePerNight <= maxPrice.Value);
                }

                ViewBag.SearchResults = filteredProducts.ToList();
            }

            // Return all products to the view
            return View(products);
        }


        // GET: Product Details
        public ActionResult Details(int productId)
        {
            var product = _unitOfWork.product.GetFirstorDefault(p => p.Id == productId, IncludeWord: "Category");

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new HostHotel
            {
                ProductId = productId,
                Product = product,
                Count = 1
            };

            return View(viewModel);
        }

        // POST: Add to HostHotel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Details(HostHotel hostHotel)
        {
            // Retrieve the current user's ID from claims
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            hostHotel.ApplicationUserId = claim?.Value;

            if (hostHotel.ApplicationUserId == null)
            {
                return Unauthorized();
            }

            // Check if the product is already in the HostHotel
            var existingHostHotel = _unitOfWork.HostHotel.GetFirstorDefault(
                h => h.ApplicationUserId == hostHotel.ApplicationUserId && h.ProductId == hostHotel.ProductId);

            if (existingHostHotel == null)
            {
                _unitOfWork.HostHotel.Add(hostHotel);
            }
            else
            {
                _unitOfWork.HostHotel.IncreaseCount(existingHostHotel, hostHotel.Count);
            }

            _unitOfWork.Complete();

            TempData["Success"] = "Product added to HostHotel successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
