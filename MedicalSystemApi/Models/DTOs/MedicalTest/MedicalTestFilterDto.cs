using System.ComponentModel.DataAnnotations;

public class MedicalTestFilterDto
{
    [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
    public string? TestName { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Minimum Cost must be a positive value.")]
    public decimal? MinCost { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Maximum Cost must be a positive value.")]
    public decimal? MaxCost { get; set; }
}

