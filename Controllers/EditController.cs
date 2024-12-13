using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCategory.Data;
using ProductsCategory.Models;

namespace ProductsCategory.Controllers
{
    public class EditController : Controller
    {
        private readonly ApplicationDBContext context;

        public EditController(ApplicationDBContext context)
        {
            this.context = context;
        }

        // HTTP GET для редактирования
        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.Products.FirstAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Передаем категории и подкатегории, сгруппированные по категориям
            ViewBag.Categories = await context.Category.ToListAsync();
            ViewBag.SubCategories = await context.SubCategory
                .Where(sc => sc.RCategoryId == product.CategoryId)
                .ToListAsync();

            return View(product);
        }

        // HTTP POST для сохранения изменений
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(product);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index", "Products");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Ошибка обновления данных: {ex.Message}");
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
