namespace MedicalSystemApi.Models.DTOs.MedicalRecord
{
    public class CreateMedicalRecordDto
    {
        public string Diagnosis { get; set; }
        public string Prescriptions { get; set; }
        public string AdditionalNotes { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
