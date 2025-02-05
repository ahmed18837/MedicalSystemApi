using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay;


        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int? StaffId { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
