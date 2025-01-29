using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; } // الاسم بالكامل

        [Required]
        //[Column(TypeName = "DATETIME")]
        [Column(TypeName = "DATE")]
        public DateTime DateOfBirth { get; set; } // تاريخ الميلاد

        [MaxLength(20)]
        public string Gender { get; set; } // النوع (ذكر/أنثى)

        [MaxLength(100)]
        public string Address { get; set; } // العنوان

        [Phone]
        public string Phone { get; set; } // رقم الهاتف

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime MedicalHistoryDate { get; set; } // التاريخ الطبي

        // Navigation Properties
        public ICollection<Appointment>? Appointments { get; set; } // Optional: مريض قد لا يكون لديه مواعيد
        public ICollection<MedicalRecord> MedicalRecords { get; set; } // Required: كل سجل طبي يجب أن يرتبط بمريض
        public ICollection<Bill> Bills { get; set; }
    }
}
