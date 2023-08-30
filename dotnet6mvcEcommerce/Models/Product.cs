using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet6mvcEcommerce.Models
{
    public class Product
    {
        [Display(Name="Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Stock")]
        public short Stock { get; set; }

        [Display(Name = "ImageUrl")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "FormFile")]
        public IFormFile? FormFile { get; set; }
    }
}
