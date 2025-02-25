using System.ComponentModel.DataAnnotations;

public class MedicalRecordFilterDto
{
    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    [MaxLength(200, ErrorMessage = "Diagnosis cannot exceed 200 characters.")]
    public string? Diagnosis { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime? StartDate { get; set; }  // Filter by CreatedAt (start date)

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime? EndDate { get; set; }    // Filter by CreatedAt (end date)
}
