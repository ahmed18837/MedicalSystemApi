using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        [DefaultValue("Pending")]
        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Patient ID is required.")]
        [DefaultValue(1111)]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        [DefaultValue(1111)]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Staff ID is required.")]
        [DefaultValue(1111)]
        public int StaffId { get; set; }
    }
}
