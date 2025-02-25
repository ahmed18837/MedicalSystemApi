namespace MedicalSystemApi.Models.DTOs.BillMedicalTest
{
    using System.ComponentModel.DataAnnotations;

    public class BillMedicalTestFilterDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Bill ID must be a valid positive integer.")]
        public int? BillId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Medical Test ID must be a valid positive integer.")]
        public int? MedicalTestId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum test cost must be a positive value.")]
        public decimal? MinTestCost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum test cost must be a positive value.")]
        public decimal? MaxTestCost { get; set; }
    }



}
