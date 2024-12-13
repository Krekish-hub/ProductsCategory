using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCategory.Data;
using ProductsCategory.Models;

namespace ProductsCategory.Controllers
{
    public class CreateController : Controller
    {
        private readonly ApplicationDBContext context;

        public CreateController(ApplicationDBContext context)
        {
            this.context = context;
        }

        // HTTP GET для отображения формы
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Передаём данные категорий и подкатегорий во ViewBag
            ViewBag.Categories = await context.Category.ToListAsync();
            ViewBag.SubCategories = await context.SubCategory.ToListAsync();

            return View();
        }

        // HTTP POST для сохранения нового продукта
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Products.Add(product);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index", "Products");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Ошибка сохранения данных: {ex.Message}");
                }
            }

            // Если есть ошибки, повторно загружаем категории и подкатегории
            ViewBag.Categories = await context.Category.ToListAsync();
            ViewBag.SubCategories = await context.SubCategory.ToListAsync();

            return View(product);
        }

        // Метод для получения списка категорий
        [HttpGet]
        public IActionResult GetCategory()
        {
            var category = context.Category.ToList();
            return Json(category);
        }

        // Метод для получения списка подкатегорий
        [HttpGet]
        public IActionResult GetSubCategory(int categoryId)
        {
            var subCategory = context.SubCategory
                .Where(sc => sc.RCategoryId == categoryId)
                .ToList();
            return Json(subCategory);
        }
    }
}
