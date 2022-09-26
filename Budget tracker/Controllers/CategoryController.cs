    using Budget_tracker.Data;
using Budget_tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget_tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BudgetDbContext _dbContext;
        public CategoryController(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();

            return categories != null ? View(categories) : Problem("categories in null");
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id)
        {
            if (id == 0)
                return View(new Category());
            else
                return View(_dbContext.Categories.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id","Title","Type","Icon")] Category category)
        {
            if (category.Id == 0)
                _dbContext.Add(category);
            else
                _dbContext.Update(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category.Id == null)
                return NotFound();
            else
                _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return View(nameof(Index));
        }
    }
}
