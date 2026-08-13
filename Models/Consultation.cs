namespace MediFlowApi.Models
{
    public class Consultation
    {
        public int Id { get; set; }
        //relation with doctor table
        public int DoctorId {set; get;}
        public Doctor Doctor { get; set; }

        public DateTime ConsultationDate {get; set;}
        public string Diagnoses {get; set;}
        public string Symptoms { get; set;}

        //relation With Patient table
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        // Soft Delete Flag
        public bool IsDeleted { get; set; } = false;

        //relation with Prescription Table
        public Prescription? Prescription { get; set; }

    }
}
