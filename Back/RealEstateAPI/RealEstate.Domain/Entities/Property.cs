namespace RealEstate.Domain.Entities;

public class Property
{
    public string Id { get; set; } = string.Empty;
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AddressProperty { get; set; } = string.Empty;
    public double PriceProperty { get; set; }
    public string Image { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public double Area { get; set; }
    public int YearBuilt { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Features { get; set; } = new();
    public int ParkingSpots { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}