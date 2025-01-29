using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystemApi.Models.Entities
{
    public class BillItem
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string ItemName { get; set; } // e.g., Lab Test, Medication

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int BillId { get; set; }

        // Relationship: Many BillItems belong to one Bill (Required)

        [ForeignKey("BillId")]
        public Bill Bill { get; set; }
    }
}
