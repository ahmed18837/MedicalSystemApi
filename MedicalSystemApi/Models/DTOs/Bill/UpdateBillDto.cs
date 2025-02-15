using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Bill
{
    public class UpdateBillDto
    {
        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, 100000, ErrorMessage = "Total amount must be between 0.01 and 100,000.")]
        [DefaultValue(500.00)] // قيمة افتراضية
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int PatientId { get; set; }
    }
}
