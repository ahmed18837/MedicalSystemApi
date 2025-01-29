using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime CreatedAt { get; set; } // تاريخ إنشاء السجل
        [MaxLength(200)]
        public string Diagnosis { get; set; } // التشخيص
        [MaxLength(100)]
        public string Prescriptions { get; set; } // الأدوية الموصوفة
        [MaxLength(200)]
        public string AdditionalNotes { get; set; }  // ملاحظات إضافية
        [Required]
        public int PatientId { get; set; } // Foreign Key
        [Required]
        public int DoctorId { get; set; } // Foreign Key

        // Navigation Properties
        public Patient Patient { get; set; } // Required: السجل الطبي يجب أن يرتبط بمريض
        public Doctor Doctor { get; set; } // Required: السجل الطبي يجب أن يرتبط بطبيب
        public ICollection<Medication> Medications { get; set; }
    }
}
