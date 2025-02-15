using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Medication
{
    public class UpdateMedicationDto
    {
        [Required(ErrorMessage = "Medication Name is required.")]
        [MaxLength(50, ErrorMessage = "Medication Name cannot exceed 50 characters.")]
        [DefaultValue("Paracetamol")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Dosage is required.")]
        [MaxLength(50, ErrorMessage = "Dosage cannot exceed 50 characters.")]
        [DefaultValue("500mg")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Medical Record ID is required.")]
        [DefaultValue(1111)]
        public int MedicalRecordId { get; set; }
    }
}
