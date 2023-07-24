using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet6mvcEcommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public short Stock { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
