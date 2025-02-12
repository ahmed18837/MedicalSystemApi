using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Auth
{
    public class RequestRegisterDto
    {
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(14)]
        [MinLength(14)]
        public string NationalId { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(64)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(64)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
