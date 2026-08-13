namespace MediFlowApi.DTOs
{
    public class ReadConsultationDto
    {
        public int Id { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }

        public string Diagnoses { get; set; }
        public string Symptoms{ get; set; }
        public  DateTime ConsultationDate {  get; set; }


    }
}
