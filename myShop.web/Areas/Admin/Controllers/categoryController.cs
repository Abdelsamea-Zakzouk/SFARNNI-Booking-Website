using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Utilities;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;


namespace myShop.web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class categoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public categoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: categoryController
        public ActionResult Index()
        {
            var Categorys = _unitOfWork.category.GetAll();

            return View(Categorys);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // _context.Categorys.Add(category);
                _unitOfWork.category.Add(category);
                // _context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Create"] = "Item Has Created";
                return RedirectToAction("Index");
            }
            return View(category);

        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            // var categoryById = _context.Categorys.Find(id);
            var categoryById = _unitOfWork.category.GetFirstorDefault(x => x.Id == id);

            return View(categoryById);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // _context.Categorys.Update(category);
                _unitOfWork.category.Update(category);
                // _context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Update"] = "Item Has Updated";
                return RedirectToAction("Index");
            }
            return View(category);

        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryById = _unitOfWork.category.GetFirstorDefault(x => x.Id == id);

            return View(categoryById);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int? id)
        {
            var categoryById = _unitOfWork.category.GetFirstorDefault(x => x.Id == id);
            if (categoryById == null)
            {
                NotFound();
            }
            // _context.Categorys.Remove(categoryById);
            _unitOfWork.category.Remove(categoryById);
            // _context.SaveChanges();
            _unitOfWork.Complete();
            TempData["Delete"] = "Item Has Deleted";
            return RedirectToAction("Index");
        }



    }
}
