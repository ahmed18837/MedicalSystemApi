using System.ComponentModel.DataAnnotations;

public class DepartmentFilterDto
{
    [StringLength(50, ErrorMessage = "Department name must be at most 50 characters.")]
    public string? Name { get; set; } // Filter by Name (Emergency, ICU, etc.)

    [StringLength(50, ErrorMessage = "Location must be at most 50 characters.")]
    public string? Location { get; set; } // Filter by Location
}
