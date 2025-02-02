namespace MedicalSystemApi.Models.DTOs.Doctor
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }
        public string WorkingHours { get; set; }
        public string DepartmentName { get; set; }
    }
}
