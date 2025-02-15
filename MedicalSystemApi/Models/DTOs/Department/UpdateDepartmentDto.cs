using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Location is required.")]
        [MaxLength(50, ErrorMessage = "Location cannot exceed 50 characters.")]
        [DefaultValue("Building A, Floor 2")]
        public string Location { get; set; }
    }
}
