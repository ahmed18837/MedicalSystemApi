using System.ComponentModel.DataAnnotations;

public class AppointmentFilterDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a valid positive integer.")]
    public int? PatientId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Doctor ID must be a valid positive integer.")]
    public int? DoctorId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Staff ID must be a valid positive integer.")]
    public int? StaffId { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime? MinDate { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime? MaxDate { get; set; }

    [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
    public string? Status { get; set; }

    // Sorting Fields
    [MaxLength(20, ErrorMessage = "OrderByField cannot exceed 20 characters.")]
    public string? OrderByField { get; set; } = "Date";

    [MaxLength(20, ErrorMessage = "OrderType cannot exceed 20 characters.")]
    public string? OrderType { get; set; } = "asc";
}
