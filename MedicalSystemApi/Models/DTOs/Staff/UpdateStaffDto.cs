using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class UpdateStaffDto : CreateStaffDto
    {
        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [DefaultValue("1990-01-01")]
        public DateTime HireDate { get; set; }
    }
}
