using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class BillMedicalTest
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key for Bill
        public int BillId { get; set; }
        [ForeignKey("BillId")]
        public Bill Bill { get; set; }

        // Foreign Key for MedicalTest
        public int MedicalTestId { get; set; }
        [ForeignKey("MedicalTestId")]
        public MedicalTest MedicalTest { get; set; }

        // Additional Attributes (Optional)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TestCost { get; set; }
    }
}
