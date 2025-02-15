using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.MedicalRecord
{
    public class UpdateMedicalRecordDto
    {
        [Required(ErrorMessage = "Diagnosis is required.")]
        [MaxLength(200, ErrorMessage = "Diagnosis cannot exceed 200 characters.")]
        [DefaultValue("General Check-up")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescriptions are required.")]
        [MaxLength(100, ErrorMessage = "Prescriptions cannot exceed 100 characters.")]
        [DefaultValue("Paracetamol 500mg - Twice a day")]
        public string Prescriptions { get; set; }

        [MaxLength(200, ErrorMessage = "Additional Notes cannot exceed 200 characters.")]
        [DefaultValue("No additional notes.")]
        public string AdditionalNotes { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [DefaultValue(1111)]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        [DefaultValue(1111)]
        public int DoctorId { get; set; }
    }
}
