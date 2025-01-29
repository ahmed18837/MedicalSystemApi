using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime DateIssued { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Relationship: Many Bills belong to one Patient (Required)
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        //// Relationship: Many Bills can be processed by one Staff (Optional)
        //public int? StaffId { get; set; }
        //[ForeignKey("StaffId")]
        //public Staff Staff { get; set; }

        // Relationship: One Bill has many BillItems (Required)
        public ICollection<BillItem> BillItems { get; set; }

        // Relationship: Many-to-Many with MedicalTest
        public ICollection<BillMedicalTest> BillMedicalTests { get; set; }

    }
}
