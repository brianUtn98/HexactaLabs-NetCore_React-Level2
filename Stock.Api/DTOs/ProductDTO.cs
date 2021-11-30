using System.ComponentModel.DataAnnotations;

namespace Stock.Api.DTOs
{
    public class ProductDTO
    {
        [Required]
        public string Name { get; set; }

        public string Id { get; set; }
        [Required]
        public string CostPrice { get; set; }
        [Required]
        public string SalePrice { get; set; }
        [Required]
        public string ProductTypeId { get; set; }
        public string ProductTypeDesc { get; set; }

    }
}