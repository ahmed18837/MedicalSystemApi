using System.ComponentModel.DataAnnotations;

public class DoctorFilterDto
{

    [StringLength(100, ErrorMessage = "Full name must be at most 100 characters.")]
    public string? FullName { get; set; }  // Filter by name

    [Range(26, 100, ErrorMessage = "Minimum age must be between 26 and 100.")]
    public int? MinAge { get; set; }       // Minimum Age

    [Range(26, 100, ErrorMessage = "Maximum age must be between 26 and 100.")]
    public int? MaxAge { get; set; }       // Maximum Age

    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female', or 'Other'.")]
    public string? Gender { get; set; }    // Male/Female/Other

    [StringLength(50, ErrorMessage = "Specialty must be at most 50 characters.")]
    public string? Specialty { get; set; } // Specialty (Cardiology, Neurology, etc.)

    [Range(1, int.MaxValue, ErrorMessage = "Department ID must be a valid positive integer.")]
    public int? DepartmentId { get; set; } // Filter by Department
}

