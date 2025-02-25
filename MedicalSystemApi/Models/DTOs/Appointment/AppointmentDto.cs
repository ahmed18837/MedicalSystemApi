namespace MedicalSystemApi.Models.DTOs.Appointment
{
    public class AppointmentDto
    {
        //public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string StaffName { get; set; }
    }
}
