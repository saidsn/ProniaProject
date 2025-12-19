using Front_To_Back_.DAL;
using Front_To_Back_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Front_To_Back_.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {

            List<Category> categories = await _context.Categories
                .Include(c=>c.Products)
                .Where(c=>c.IsDeleted==false)
                .ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //home //home 

            bool existCategory = await _context.Categories.AnyAsync(c=>c.Name.Trim() == category.Name.Trim());


            if (existCategory)
            {
                ModelState.AddModelError("Name", "Category already exists!");
                return View();
            }

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            //return View("Index");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);

            if (category is null) return NotFound(); 

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (id is null || id < 1) return BadRequest();

            Category existedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            if(!ModelState.IsValid) return View();

            bool result = await _context.Categories.AnyAsync(c=>c.Name.Trim() == category.Name.Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), "Category already exists!");
                return View();
            } 

            existedCategory.Name = category.Name;

            //await _context.Categories.Update();
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public  async Task<IActionResult> Delete(int? id)
        {
            if(id is null || id < 1 ) return BadRequest();

            Category existCategory = await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);

            if (existCategory is null) return NotFound();

            if (existCategory.IsDeleted == false)
            {
                existCategory.IsDeleted = true;
            }
            else
            {
                existCategory.IsDeleted = false;
            }



                //_context.Categories.Remove(existCategory);

                await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
