using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.Models
{
    public class Prescription
    {
        
        public int Id { get; set; }
        public int DurationInDays { get; set; }
        public ICollection<PrescriptionItem> PrescriptionItems { get; set; }
        =new List<PrescriptionItem>();

        public Consultation Consultation { get; set; }
        [Required]
        public int  ConsultationId { get; set; }

    }
}
