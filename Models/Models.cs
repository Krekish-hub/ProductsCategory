using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsCategory.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }

    [Table("SubCategory")]
    public class SubCategory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("categoryId")]
        public int RCategoryId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        public Category Category { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("categoryId")]
        public int CategoryId { get; set; }

        [Column("subCategoryId")]
        public int SubCategoryId { get; set; }

        [Column("productName")]
        public string? ProductName { get; set; }

        [Column("price ($)")]
        public decimal? Price { get; set; }

        [Column("discount (%)")]
        public decimal? Discount { get; set; }

        [Column("specialPrice ($)")]
        public decimal? SpecialPrice { get; set; }

        [Column ("imageFileName")]
        public string? ImageFileName { get; set; }

        // Вычисляемое свойство (не сохраняется в базе)
        //[NotMapped]
        //public string? FullProductName => $"{ProductName}, {ProductDetails?.Unit}";


        public Category? Category { get; set; }
        public SubCategory? SubCategory { get; set; }
    }

    public class ProductDetails
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("productId")]
        public string ProductId { get; set; }

        [Column("productName")]
        public string ProductName { get; set; }

        [Column("weight")]
        public decimal? Weight { get; set; }

        [Column("unit")]
        public string? Unit { get; set; }
    }
}
