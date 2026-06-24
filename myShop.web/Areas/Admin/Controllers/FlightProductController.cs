using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Utilities;
using myShop.Entities.Models;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class FlightProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FlightProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var flightProducts = _unitOfWork.flightProduct.GetAll(IncludeWord: "Category");
            return Json(new { data = flightProducts });
        }

        [HttpGet]
        public IActionResult Create()
        {
            FlightProductVM flightProductVM = new FlightProductVM()
            {
                FlightProduct = new FlightProduct(),
                CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(flightProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FlightProductVM flightProductVM)
        {
            if (ModelState.IsValid)
            {
                // Add the FlightProduct directly without image handling
                _unitOfWork.flightProduct.Add(flightProductVM.FlightProduct);
                _unitOfWork.Complete();
                TempData["Create"] = "Flight Product has been created successfully";
                return RedirectToAction("Index");
            }
            // If model is invalid, re-populate the CategoryList
            flightProductVM.CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(flightProductVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            FlightProductVM flightProductVM = new FlightProductVM()
            {
                FlightProduct = _unitOfWork.flightProduct.GetFirstorDefault(x => x.Id == id),
                CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(flightProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FlightProductVM flightProductVM)
        {
            if (ModelState.IsValid)
            {
                // Update the FlightProduct directly without image handling
                _unitOfWork.flightProduct.Update(flightProductVM.FlightProduct);
                _unitOfWork.Complete();
                TempData["Update"] = "Flight Product has been updated successfully";
                return RedirectToAction("Index");
            }
            // If model is invalid, re-populate the CategoryList
            flightProductVM.CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(flightProductVM);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var flightProductInDb = _unitOfWork.flightProduct.GetFirstorDefault(x => x.Id == id);
            if (flightProductInDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.flightProduct.Remove(flightProductInDb);
            _unitOfWork.Complete();
            return Json(new { success = true, message = "Flight product has been deleted" });
        }
    }
}
