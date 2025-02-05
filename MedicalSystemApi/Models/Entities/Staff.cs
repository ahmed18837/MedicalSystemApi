using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class Staff
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [MaxLength(100)]
        public string FullName { get; set; } // الاسم بالكامل

        [MaxLength(50)]
        public string RoleStaff { get; set; } // الدور (سكرتير، موظف استقبال، إلخ)

        [StringLength(15)]
        public string Phone { get; set; } // رقم الهاتف

        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } // البريد الإلكتروني

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime HireDate { get; set; } // تاريخ التوظيف

        public string? ImagePath { get; set; }

        // Navigation Properties
        [Required]
        public int DepartmentId { get; set; } // Foreign Key للقسم
        public Department Department { get; set; } // علاقة Many-to-One مع Department

        public ICollection<Appointment>? Appointments { get; set; } // الموظف قد يتعامل مع مواعيد
    }
}
