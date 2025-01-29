using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.Entities
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } // اسم الدواء
        [MaxLength(50)]
        public string Dosage { get; set; } // الجرعة
        [MaxLength(200)]
        public string Frequency { get; set; } // تكرار الجرعة (مثل "مرة يومياً")
        [MaxLength(50)]
        public string Route { get; set; } // طريق الإعطاء (مثل "فموي")
        [MaxLength(200)]
        public string Instructions { get; set; } // تعليمات الاستخدام
        [Required]
        public int MedicalRecordId { get; set; } // Foreign Key مرتبط بالسجل الطبي

        // Navigation Properties
        public MedicalRecord MedicalRecord { get; set; } // Optional: بعض السجلات قد لا تحتوي على وصفات
    }
}
