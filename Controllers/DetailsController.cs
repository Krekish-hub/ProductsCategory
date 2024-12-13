using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCategory.Data;

namespace ProductsCategory.Controllers
{
    public class DetailsController : Controller
    {
        private readonly ApplicationDBContext context;

        public DetailsController(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            // Передаём данные категорий и подкатегорий во ViewBag
            ViewBag.Categories = await context.Category.ToListAsync();
            ViewBag.SubCategories = await context.SubCategory.ToListAsync();

            // Получаем Id, по которому перешли
            var product = await context.Products.FirstAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
