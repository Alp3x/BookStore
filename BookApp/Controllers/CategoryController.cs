using BookApp.Data;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //  ModelState.AddModelError("", "The Display order cant match the name");
            //}
            //if (obj.Name != null && (obj.Name == "test" || obj.Name == "Teste"))
            //{
            //	ModelState.AddModelError("","test is an Invalid Name");
            //}
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
				TempData["success"] = "Category created sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }
		public IActionResult Edit(int id)
		{
            if( id == 0)
            {
                return NotFound();
            }
            // 3 Formas de Tirar um valor da Base de Dados
            Category? categoryFromDb = _db.Categories.Find(id);
			//Category? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
			//Category? categoryFromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
			if (categoryFromDb == null)
            {
                return NotFound();
            }
			return View(categoryFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Category updated sucessfully";
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
			Category? categoryFromDb = _db.Categories.Find(id);
			//Category? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
			//Category? categoryFromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
			if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}

		[HttpPost,ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? obj = _db.Categories.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.Categories.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
