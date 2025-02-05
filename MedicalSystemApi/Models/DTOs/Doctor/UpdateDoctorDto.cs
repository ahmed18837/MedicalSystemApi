namespace MedicalSystemApi.Models.DTOs.Doctor
{
    public class UpdateDoctorDto
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }
        public string WorkingHours { get; set; }
        public int DepartmentId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
