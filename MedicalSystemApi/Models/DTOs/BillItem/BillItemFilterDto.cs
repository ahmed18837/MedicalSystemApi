using System.ComponentModel.DataAnnotations;

public class BillItemFilterDto
{
    [StringLength(100, ErrorMessage = "Item name must be at most 100 characters.")]
    public string? ItemName { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be a positive value.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be a positive value.")]
    public decimal? MaxPrice { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity must be at least 1.")]
    public int? MinQuantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Maximum quantity must be at least 1.")]
    public int? MaxQuantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Bill ID must be a valid positive integer.")]
    public int? BillId { get; set; }

    // Sorting Fields
    [RegularExpression("^(Id|ItemName|Price|Quantity|BillId)$", ErrorMessage = "Invalid sorting field.")]
    public string? OrderByField { get; set; } = "Id"; // Default sorting field

    [RegularExpression("^(asc|desc)$", ErrorMessage = "Order type must be 'asc' or 'desc'.")]
    public string? OrderType { get; set; } = "asc"; // "asc" or "desc"
}


