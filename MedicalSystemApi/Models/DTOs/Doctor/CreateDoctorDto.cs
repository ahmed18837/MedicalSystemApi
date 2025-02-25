using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        [DefaultValue("FullName")] // قيمة افتراضية
        public string FullName { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(26, 100, ErrorMessage = "Age must be between 26 and 100.")]
        [DefaultValue(30)] // قيمة افتراضية
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(50, ErrorMessage = "Gender cannot exceed 50 characters.")]
        [DefaultValue("Male")] // قيمة افتراضية
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [DefaultValue("+201234567890")] // قيمة افتراضية
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [DefaultValue("doctor@example.com")] // قيمة افتراضية
        public string Email { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        [MaxLength(50, ErrorMessage = "Specialty cannot exceed 50 characters.")]
        [DefaultValue("Cardiology")] // قيمة افتراضية
        public string Specialty { get; set; }

        [Required(ErrorMessage = "Working hours are required.")]
        [MaxLength(100, ErrorMessage = "Working hours cannot exceed 100 characters.")]
        [DefaultValue("09:30 AM - 05:30 PM")] // قيمة افتراضية
        public string WorkingHours { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int DepartmentId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
