using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.DTOs
{
    public class CreateConsultationDto
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int PatientId { get; set; }
      
        [Required]
        [MaxLength(500)]
        public string Diagnoses { get; set; }
        [Required]
     
        [MaxLength(500)]
        public string Symptoms { get; set; }

    }
}
