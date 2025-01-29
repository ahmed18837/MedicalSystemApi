using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } // اسم القسم (طوارئ، عناية مركزة، إلخ)

        [MaxLength(50)]
        public string Location { get; set; } // موقع القسم في المستشفى

        // Navigation Properties
        public ICollection<Doctor> Doctors { get; set; } // Required: كل طبيب ينتمي إلى قسم
        public ICollection<Staff> Staffs { get; set; } // علاقة One-to-Many مع Staff

    }
}
