using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.BillMedicalTest
{
    public class CreateBillMedicalTestDto
    {
        [Required(ErrorMessage = "Bill ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int BillId { get; set; }

        [Required(ErrorMessage = "Medical Test ID is required.")]
        [DefaultValue(1111)] // قيمة افتراضية
        public int MedicalTestId { get; set; }

        [Required(ErrorMessage = "Test cost is required.")]
        [Range(0.01, 10000, ErrorMessage = "Test cost must be between 0.01 and 10,000.")]
        [DefaultValue(100.00)] // قيمة افتراضية
        public decimal TestCost { get; set; }
    }
}
