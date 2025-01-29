using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class MedicalTest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string TestName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        // Relationship: Many-to-Many with Bill
        public ICollection<BillMedicalTest> BillMedicalTests { get; set; }

    }
}
