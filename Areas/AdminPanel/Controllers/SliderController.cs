using Front_To_Back_.DAL;
using Front_To_Back_.Models;
using Front_To_Back_.Utilities.Enums;
using Front_To_Back_.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Front_To_Back_.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!slider.Photo.ValidatorType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is incorrect!");
                return View();
            }

            if (!slider.Photo.ValidatorSize(FileSize.MB, slider.Photo.Length))
            {
                ModelState.AddModelError("Photo", "File size must be less than 2 mb");
                return View();
            }

            if(!ModelState.IsValid) return View();

            slider.ImageURL = await slider.Photo.CreateFileAsync(_env.WebRootPath,"assets","Images","website-images");

            await _context.Sliders.AddAsync(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slider slider  = await _context.Sliders.FirstOrDefaultAsync(s=>s.Id == id);

            if (slider == null) return NotFound();

            slider.ImageURL.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");

            _context.Sliders.Remove(slider);

            await _context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Index));
      
        }

    }
}
