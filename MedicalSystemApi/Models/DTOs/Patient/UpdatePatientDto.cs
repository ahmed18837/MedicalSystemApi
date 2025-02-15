using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Patient
{
    public class UpdatePatientDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        [DefaultValue("Ahmed Ahmed")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        [DefaultValue("Cairo, Egypt")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [DefaultValue("+201234567891")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Medical History Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [DefaultValue("2024-01-01")]
        public DateTime MedicalHistoryDate { get; set; }
    }
}
