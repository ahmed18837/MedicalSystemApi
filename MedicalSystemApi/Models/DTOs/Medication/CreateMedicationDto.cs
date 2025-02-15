using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Medication
{
    public class CreateMedicationDto
    {
        [Required(ErrorMessage = "Medication Name is required.")]
        [MaxLength(50, ErrorMessage = "Medication Name cannot exceed 50 characters.")]
        [DefaultValue("Paracetamol")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Dosage is required.")]
        [MaxLength(50, ErrorMessage = "Dosage cannot exceed 50 characters.")]
        [DefaultValue("500mg")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Instructions are required.")]
        [MaxLength(200, ErrorMessage = "Instructions cannot exceed 200 characters.")]
        [DefaultValue("Take after food with a glass of water.")]
        public string Instructions { get; set; }

        [Required(ErrorMessage = "Frequency is required.")]
        [MaxLength(200, ErrorMessage = "Frequency cannot exceed 200 characters.")]
        [DefaultValue("Twice daily")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Route of administration is required.")]
        [MaxLength(50, ErrorMessage = "Route cannot exceed 50 characters.")]
        [DefaultValue("Oral")]
        public string Route { get; set; }

        [Required(ErrorMessage = "Medical Record ID is required.")]
        [DefaultValue(1111)]
        public int MedicalRecordId { get; set; }
    }
}
