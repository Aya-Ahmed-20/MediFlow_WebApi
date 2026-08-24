using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.Models
{
    public class PrescriptionItem
    {
        public int Id { get; set; }
        [Required]
        public string Dose { get; set; }
        public string? Instructions { get; set; }
        //relation with prescription
        public Prescription Prescription { get; set; }
        public int PrescriptionId { get; set; }
        //relation with Medicine Table
        public Medicine Medicine { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
