using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.MedicalTest
{
    public class UpdateMedicalTestDto
    {
        [Required(ErrorMessage = "Cost is required.")]
        [Range(0.01, 10000, ErrorMessage = "Cost must be between 0.01 and 10,000.")]
        [DefaultValue(100.00)]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [DefaultValue("A routine blood test to check overall health.")] // Default value
        public string Description { get; set; }
    }
}
