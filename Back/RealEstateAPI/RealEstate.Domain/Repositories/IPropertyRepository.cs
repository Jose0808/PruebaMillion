using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Repositories;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(string id);
    Task<(IEnumerable<Property> Properties, int TotalCount, int TotalPages)> GetByFiltersAsync(PropertyFilter filter);
}

public class PropertyFilter
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}