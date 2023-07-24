using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet6mvcEcommerce.Models
{
    public class Product
    {
        [Display(Name="商品Id")]
        public int Id { get; set; }

        [Display(Name = "商品名稱")]
        public string Name { get; set; }

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "庫存")]
        public short Stock { get; set; }

        [Display(Name = "圖片URL")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "圖片")]
        public IFormFile? FormFile { get; set; }
    }
}
