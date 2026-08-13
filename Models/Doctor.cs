namespace MediFlowApi.Models
{
    public class Doctor
    {
        //Basic Data
        public int Id { set; get; }
        public string Name { set; get; }    

        public string Specialization {  set; get; }
        public int LicenseNumber { set; get; }
        public int YearOfExperience { set; get; }
        // Relationship with Identity User
        public string ApplicationUserId {  set; get; }//GUid
        public ApplicationUser user {  set; get; }
        //Relationship with Consultation table
        public ICollection<Consultation>Consultations { set; get; }

    }
}
