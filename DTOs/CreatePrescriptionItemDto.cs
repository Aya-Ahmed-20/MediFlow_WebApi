namespace MediFlowApi.DTOs
{
    public class CreatePrescriptionItemDto
    {
        public string Dose { get; set; }
        public string? Instructions { get; set; }
        public int MedicineId { get; set; }
    }
}
