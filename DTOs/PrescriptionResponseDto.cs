namespace MediFlowApi.DTOs
{
    public class PrescriptionResponseDto
    {
        public int Id { get; set; }
        public int ConsultationId { get; set; }
        public int DurationInDays { get; set; }
        public List<PrescriptionItemDto> PrescriptionItems { get; set; }
    }

    public class PrescriptionItemDto
    {
        public int MedicineId { get; set; }
        public string Dose { get; set; }
        public string Instructions { get; set; }
    }
}
