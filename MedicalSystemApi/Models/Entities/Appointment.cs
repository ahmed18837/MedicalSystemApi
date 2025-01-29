using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime Date { get; set; } // تاريخ الموعد

        [Required]
        [Column(TypeName = "TIME")]
        public TimeSpan Time { get; set; } // وقت الموعد

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // حالة الموعد (Pending, Confirmed, Cancelled)

        [MaxLength(255)]
        public string Notes { get; set; } // ملاحظات إضافية

        public int? PatientId { get; set; } // Foreign Key (Required: الموعد يجب أن يرتبط بمريض)

        public int? DoctorId { get; set; } // Foreign Key (Required: الموعد يجب أن يرتبط بطبيب)

        public int? StaffId { get; set; }

        // Navigation Properties
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } // Required:
                                             // الموعد يجب أن يرتبط بمريض
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } // Required: الموعد يجب أن يرتبط بطبيب

        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }
    }
}
