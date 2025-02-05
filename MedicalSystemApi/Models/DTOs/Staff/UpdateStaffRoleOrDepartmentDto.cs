using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class UpdateStaffRoleOrDepartmentDto
    {
        [Required]
        public int StaffId { get; set; }

        public string RoleStaff { get; set; }

        public int DepartmentId { get; set; }
    }
}
