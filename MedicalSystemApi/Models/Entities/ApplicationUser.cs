using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string NationalId { get; set; }

        [MaxLength(8)]
        public string? ResetCode { get; set; }
    }
}
