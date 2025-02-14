using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Auth
{
    public class ChangePasswordDto
    {
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(64)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(64)]
        public string NewPassword { get; set; }
    }
}
