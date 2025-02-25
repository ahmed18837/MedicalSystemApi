using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class StaffFilterDto
    {
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string? FullName { get; set; }

        [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string? RoleStaff { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? HireDateFrom { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? HireDateTo { get; set; }

        public int? DepartmentId { get; set; }
    }

}
