using Front_To_Back_.DAL;
using Front_To_Back_.Models;
using Front_To_Back_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Front_To_Back_.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context) 
        {
            _context = context;
        }

        // null false true 

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Product? product = await _context.Products
                .Include(p=>p.ProductImages.OrderByDescending(pi=>pi.IsPrimary))
                .Include(p=>p.Category)
                .FirstOrDefaultAsync(p=>p.Id == id);

            if(product == null) return NotFound();

            List<Product> relatedProducts = _context.Products
                .Include(p=>p.ProductImages.Where(pi=>pi.IsPrimary != null))
                .Where(p=>p.CategoryId == product.CategoryId && p.Id != id)
                .Take(2)
                .ToList();

            DetailVM detailVM = new DetailVM
            { 
                Product = product,
                RelatedProducts = relatedProducts
            };


            return View(detailVM);
        }
    }
}
