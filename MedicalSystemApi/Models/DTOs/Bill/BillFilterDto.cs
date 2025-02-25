using System.ComponentModel.DataAnnotations;

namespace MedicalSystemApi.Models.DTOs.Bill
{
    public class BillFilterDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a valid positive integer.")]
        public int? PatientId { get; set; } // Filter by Patient ID

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? MinDateIssued { get; set; } // Filter by minimum issue date

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? MaxDateIssued { get; set; } // Filter by maximum issue date

        [Range(0, double.MaxValue, ErrorMessage = "Minimum amount must be a positive value.")]
        public decimal? MinTotalAmount { get; set; } // Minimum total amount

        [Range(0, double.MaxValue, ErrorMessage = "Maximum amount must be a positive value.")]
        public decimal? MaxTotalAmount { get; set; } // Maximum total amount

        // Sorting Fields
        [RegularExpression("^(Id|DateIssued|TotalAmount|PatientId)$", ErrorMessage = "Invalid sorting field.")]
        public string? OrderByField { get; set; } = "Id"; // Default sorting field

        [RegularExpression("^(asc|desc)$", ErrorMessage = "Order type must be 'asc' or 'desc'.")]
        public string? OrderType { get; set; } = "asc"; // "asc" or "desc"
    }

}
