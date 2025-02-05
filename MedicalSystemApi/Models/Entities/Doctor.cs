using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; } // الاسم بالكامل

        [Range(26, 100)]
        public int Age { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [Phone]
        public string Phone { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } // البريد الإلكتروني

        [MaxLength(50)]
        public string Specialty { get; set; } // التخصص (قلب، أعصاب، أطفال، إلخ)

        [Required]
        [MaxLength(100)]
        public string WorkingHours { get; set; } // ساعات العمل

        [Required]
        public int? DepartmentId { get; set; } // Foreign Key (Required: الطبيب ينتمي لقسم محدد)

        // Navigation Properties
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; } // Required
        public ICollection<Appointment>? Appointments { get; set; } // Optional: طبيب قد لا يكون لديه مواعيد
        public ICollection<MedicalRecord> MedicalRecords { get; set; } // Required: كل سجل طبي يرتبط بطبيب
    }
}
