namespace MedicalSystemApi.Models.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        public string Status { get; set; } = "Pending";
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? StaffId { get; set; }
    }
}
