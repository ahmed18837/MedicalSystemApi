namespace MedicalSystemApi.Models.DTOs.Medication
{
    public class UpdateMedicationDto
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
        public int MedicalRecordId { get; set; }
    }
}
