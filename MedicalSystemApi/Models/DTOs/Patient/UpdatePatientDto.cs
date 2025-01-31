namespace MedicalSystemApi.Models.DTOs.Patient
{
    public class UpdatePatientDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime MedicalHistoryDate { get; set; }
    }
}
