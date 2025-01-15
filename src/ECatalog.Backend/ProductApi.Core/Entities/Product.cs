using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Core.Entities
{
    public class Product
    {
        [Key]
        [Required]
        [RegularExpression(EntitiesRegex.ProductRegex, ErrorMessage = "Code must be in the format {XXXX-XXXX}, where X is a digit.")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters long.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can be up to 500 characters long.")]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
