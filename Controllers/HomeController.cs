using Front_To_Back_;
using Front_To_Back_.DAL;
using Front_To_Back_.Models;
using Front_To_Back_.ViewModels;
using FrontToBack.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Controllers
{

    // DI Dependdecy Injection

    // IOC  Inversve of control  // DIP Dependecy Inversion Priciple

    // DC Dependency Container
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _context.Sliders
                .OrderBy(s=>s.Order)
                .ToListAsync();
            List<Shipping> shippings = await _context.Shippings
                .ToListAsync();
            List<Product> products = await _context.Products
                .Include(p => p.ProductImages.Where(pi=>pi.IsPrimary != null))
                .ToListAsync();


            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                Shippings = shippings,
                Products = products
            };


            return View(homeVM);
        }
    }
}

