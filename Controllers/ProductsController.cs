using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProductsCategory.Data;
using ProductsCategory.Models;

namespace ProductsCategory.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public int Id { get; set; }

        public ProductsController(ILogger<ProductsController> logger, ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string sortOrder, int? categoryId, int? subCategoryId)
        {
            ViewBag.Categories = await context.Category.ToListAsync();
            ViewBag.SubCategories = await context.SubCategory.ToListAsync();

            var products = context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (subCategoryId.HasValue)
            {
                products = products.Where(p => p.SubCategoryId == subCategoryId.Value);
            }

            switch (sortOrder)
            {
                case "discount_asc":
                    products = products.OrderBy(p => p.Discount);
                    break;
                case "discount_desc":
                    products = products.OrderByDescending(p => p.Discount);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Id);
                    break;
            }

            var productList = await products
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .ToListAsync();

            return View(productList);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await context.Products.FindAsync(productId);

            if (product != null)
            {
                // Если хотите также удалить изображение с диска, раскомментируйте этот блок
                // var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", product.ImageFileName);
                // if (System.IO.File.Exists(imagePath))
                // {
                //     System.IO.File.Delete(imagePath);
                // }

                // Удаление записи о продукте из базы данных
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Products");
        }

    }
}

