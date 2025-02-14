using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Auth
{
    public class Verify2FACodeDto
    {
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(8)]
        public string Code { get; set; }
    }
}
