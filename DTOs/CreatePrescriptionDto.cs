namespace MediFlowApi.DTOs
{
    public class CreatePrescriptionDto
    {
        public int DurationInDays { get; set; }
        public int ConsultationId { get; set; }
        public List<CreatePrescriptionItemDto> PrescriptionItems { get; set; }=new List<CreatePrescriptionItemDto>();

    }
}
