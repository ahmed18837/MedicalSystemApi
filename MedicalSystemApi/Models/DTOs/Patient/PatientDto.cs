namespace MedicalSystemApi.Models.DTOs.Patient
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime MedicalHistoryDate { get; set; }
    }
}
