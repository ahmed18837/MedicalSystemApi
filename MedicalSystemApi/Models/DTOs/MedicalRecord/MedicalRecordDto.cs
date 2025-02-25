namespace MedicalSystemApi.Models.DTOs.MedicalRecord
{
    public class MedicalRecordDto
    {
        //public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Diagnosis { get; set; }
        public string Prescriptions { get; set; }
        public string AdditionalNotes { get; set; }
        public int PatientId { get; set; }
        public string DoctorName { get; set; }
    }
}
