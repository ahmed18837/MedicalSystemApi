using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.MedicalRecord
{
    public class CreateMedicalRecordDto
    {
        [Required(ErrorMessage = "Diagnosis is required.")]
        [MaxLength(200, ErrorMessage = "Diagnosis cannot exceed 200 characters.")]
        [DefaultValue("General Check-up")] // قيمة افتراضية
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescriptions are required.")]
        [MaxLength(255, ErrorMessage = "Prescriptions cannot exceed 100 characters.")]
        [DefaultValue("Paracetamol 500mg - Twice a day")] // قيمة افتراضية
        public string Prescriptions { get; set; }

        [MaxLength(200, ErrorMessage = "Additional Notes cannot exceed 200 characters.")]
        [DefaultValue("No additional notes.")] // قيمة افتراضية
        public string AdditionalNotes { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int DoctorId { get; set; }
    }
}
