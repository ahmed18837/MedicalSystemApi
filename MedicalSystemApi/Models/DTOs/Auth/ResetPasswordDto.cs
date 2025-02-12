using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(8)]
        public string? ResetCode { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(50)]
        public string NewPassword { get; set; }
    }
}
