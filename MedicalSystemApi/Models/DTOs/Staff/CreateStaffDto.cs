using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class CreateStaffDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        [DefaultValue("Ahmed Ahmed")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        [DefaultValue("Doctor")]
        public string RoleStaff { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [DefaultValue("+201234567891")]
        public string Phone { get; set; } = "+201234567891";

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [DefaultValue(1111)]
        public int DepartmentId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
