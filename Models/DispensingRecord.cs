namespace MediFlowApi.Models
{
    public class DispensingRecord
    {
        public int Id {  get; set; }

        public int PrescriptionId { get; set; }

        public string PharmacistId { get; set; }

        public DateTime DispensedAt { get; set; }

        public string? Notes { get; set; }
        //Navigation property
        public Prescription Prescription { get; set; }
        //navigation property
        public ApplicationUser Pharmacist { get; set; } = null!;
        

    }
}
