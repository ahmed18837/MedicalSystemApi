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

        [MaxLength(8)]
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorCodeExpiration { get; set; }
        public DateTime? TwoFactorSentAt { get; set; } // 🔹 وقت إرسال الكود الأخير

        public int TwoFactorAttempts { get; set; } = 0; // عدد المحاولات خلال الساعة
        public DateTime? LastTwoFactorAttempt { get; set; } // آخر وقت للمحاولة

        // 🔹 جديد: محاولات الفشل وقفل الحساب
        public int FailedTwoFactorAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; } // متى ينتهي القفل؟
    }
}
