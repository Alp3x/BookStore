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
   //         if(obj.Name == obj.DisplayOrder.ToString())
   //         {
   //             ModelState.AddModelError("", "The Display order cant match the name");
   //         }
			//if (obj.Name != null && (obj.Name == "test" || obj.Name == "Teste"))
			//{
			//	ModelState.AddModelError("","test is an Invalid Name");
			//}
			if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
		}
	}
}
