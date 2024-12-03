using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModel;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_UnitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }
        public IActionResult UpSert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                Value = u.Id.ToString()
                });

            //ViewBag.CategoryList = CategoryList;
			//ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {

                CategoryList = CategoryList,
                Product = new Product()
            };

            if(id == null || id == 0 )
            {
                // Create
				return View(productVM);
			}
            else
            {
                //Update
                productVM.Product = _UnitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }


        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName =Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!String.IsNullOrEmpty(productVm.Product.ImageUrl)) 
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\');

                        if(System.IO.File.Exists(oldImagePath)){
                        System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVm.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVm.Product.Id == 0) 
                {
					_UnitOfWork.Product.Add(productVm.Product);
				}
                else
                {
                    _UnitOfWork.Product.Update(productVm.Product);
                }
                _UnitOfWork.Save();
                TempData["success"] = "Product created sucessfully";
                return RedirectToAction("Index");
            }
            else 
            {
				//ViewBag.CategoryList = CategoryList;
				//ViewData["CategoryList"] = CategoryList;
				productVm.CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
					{
						Text = u.Name,
						Value = u.Id.ToString()
					});
				return View(productVm);
			}
            
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
