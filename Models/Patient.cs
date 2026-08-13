namespace MediFlowApi.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int age {  get; set; }
        public string MedicalHistory { get; set; }

        public string BloodType {  get; set; }
        //relation with ApplicationUserTable
        public string ApplicatioUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //Relation with Consultation table one-to-many relationship
        public ICollection <Consultation> consultations { get; set; }

    }
}
