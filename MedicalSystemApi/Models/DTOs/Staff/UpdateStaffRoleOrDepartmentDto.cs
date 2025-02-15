using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class UpdateStaffRoleOrDepartmentDto
    {
        [Required(ErrorMessage = "Staff ID is required.")]
        [DefaultValue(1111)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = "Role Staff is required.")]
        [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        [DefaultValue("Doctor")]
        public string RoleStaff { get; set; }

        [Required(ErrorMessage = "Department ID is required.")]
        [DefaultValue(1111)]
        public int DepartmentId { get; set; }
    }
}
