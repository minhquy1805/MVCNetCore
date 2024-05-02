using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVC.Data;
using WebMVC.Models;

namespace WebMVC.Controllers
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
        [HttpGet]
        public async Task<IActionResult> Index(string searchingString)
        {
            ViewData["GetCategoryDetail"] = searchingString;
            var cateQuery = from x in _db.Categories select x;
            if (!String.IsNullOrEmpty(searchingString))
            {
                cateQuery = cateQuery.Where(x => x.Name.Contains(searchingString));
            }
            return View(await cateQuery.AsNoTracking().ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder must not match with Name");
            }
            if(ModelState.IsValid) {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
            
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
          
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? category = _db.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
       
    }
}
    

