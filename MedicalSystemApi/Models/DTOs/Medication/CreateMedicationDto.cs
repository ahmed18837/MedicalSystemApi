namespace MedicalSystemApi.Models.DTOs.Medication
{
    public class CreateMedicationDto
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public int MedicalRecordId { get; set; }
    }
}
