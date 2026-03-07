using PCBuilder.Models;

namespace PCBuilder.ViewModels;

public class PartListItem
{
    public BasePart Part { get; set; } = null!;
    public string Name { get; set; } = "";
    public string ManufacturerName { get; set; } = "";
    public decimal Price { get; set; }
    public string Specs { get; set; } = "";
    public string PriceDisplay => $"{Price:N0} ₽";
}
