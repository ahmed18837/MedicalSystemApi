using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Department
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(50, ErrorMessage = "Department name cannot exceed 50 characters.")]
        [DefaultValue("Emergency")] // قيمة افتراضية
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [MaxLength(50, ErrorMessage = "Location cannot exceed 50 characters.")]
        [DefaultValue("Building A, Floor 2")] // قيمة افتراضية
        public string Location { get; set; }
    }
}
