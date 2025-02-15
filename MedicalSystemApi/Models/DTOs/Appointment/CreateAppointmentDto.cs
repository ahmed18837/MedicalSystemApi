using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "Appointment date is required.")]
        [DefaultValue("2025-02-15")]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Required(ErrorMessage = "Appointment time is required.")]
        [DefaultValue("10:00:00")]
        public TimeSpan Time { get; set; } = new TimeSpan(10, 0, 0);

        [Required(ErrorMessage = "Patient ID is required.")]
        [DefaultValue(1111)]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        [DefaultValue(1111)]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Staff ID is required.")]
        [DefaultValue(1111)]
        public int StaffId { get; set; }

        [MaxLength(255)]
        [DefaultValue("Routine check-up")]
        public string Notes { get; set; }

        [DefaultValue("Pending")]
        public string Status { get; set; } = "Pending";
    }
}
