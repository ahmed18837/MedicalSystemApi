using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Medication
{
    public class MedicationFilterDto
    {
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [MaxLength(50, ErrorMessage = "Dosage cannot exceed 50 characters.")]
        public string? Dosage { get; set; }

        [MaxLength(200, ErrorMessage = "Frequency cannot exceed 200 characters.")]
        public string? Frequency { get; set; }

        [MaxLength(50, ErrorMessage = "Route cannot exceed 50 characters.")]
        public string? Route { get; set; }

        [DefaultValue(1111)]
        public int? MedicalRecordId { get; set; }
    }

}
