using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace BookApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                Value = u.Id.ToString()
                });

            //ViewBag.CategoryList = CategoryList;
			ViewData["CategoryList"] = CategoryList;

			return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Product.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Product created sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            // 3 Formas de Tirar um valor da Base de Dados
            Product? ProductFromDb = _UnitOfWork.Product.Get(u => u.Id == id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Product.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Product updated sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            // 3 Formas de Tirar um valor da Base de Dados
            Product? productFromDb = _UnitOfWork.Product.Get(u => u.Id == id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _UnitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _UnitOfWork.Product.Remove(obj);
            _UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
