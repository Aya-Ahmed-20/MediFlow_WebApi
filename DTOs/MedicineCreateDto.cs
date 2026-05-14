using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.DTOs
{
    public class MedicineCreateDto
    {
        [Required(ErrorMessage = "Enter Medicine Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage ="لازم السعر يكون موجب"), Range(0.1, 100000)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }

    }
}
