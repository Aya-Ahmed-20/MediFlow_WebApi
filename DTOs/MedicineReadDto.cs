using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.DTOs
{
    public class MedicineReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }

    }
}
