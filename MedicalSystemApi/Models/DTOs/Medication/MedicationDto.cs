namespace MedicalSystemApi.Models.DTOs.Medication
{
    public class MedicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public int MedicalRecordId { get; set; }
    }
}
