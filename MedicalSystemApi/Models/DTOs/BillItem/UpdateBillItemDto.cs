using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.BillItem
{
    public class UpdateBillItemDto
    {
        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        [DefaultValue("Lab Test")] // قيمة افتراضية
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000.")]
        [DefaultValue(200.00)] // قيمة افتراضية
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        [DefaultValue(100)] // قيمة افتراضية
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Bill ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int BillId { get; set; }
    }
}

