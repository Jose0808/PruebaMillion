using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Data.Entities;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<MongoProperty> _properties;

    public PropertyRepository(IMongoDbContext context)
    {
        _properties = context.Properties ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        var mongoProperties = await _properties.Find(_ => true).ToListAsync();
        return mongoProperties.Select(mp => mp.ToDomainEntity());
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var mongoProperty = await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
        return mongoProperty?.ToDomainEntity();
    }
    public async Task<(IEnumerable<Property> Properties, int TotalCount, int TotalPages)> GetByFiltersAsync(PropertyFilter filter)
    {
        // Validar parámetros de paginación
        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

        var combinedFilter = BuildMongoFilter(filter);

        // Total de registros
        var totalCount = (int)await _properties.CountDocumentsAsync(combinedFilter);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Datos paginados
        var mongoProperties = await _properties
            .Find(combinedFilter)
            .SortBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        var domainProperties = mongoProperties.Select(mp => mp.ToDomainEntity());

        return (domainProperties, totalCount, totalPages);
    }

    private static FilterDefinition<MongoProperty> BuildMongoFilter(PropertyFilter filter)
    {
        var fb = Builders<MongoProperty>.Filter;
        var filters = new List<FilterDefinition<MongoProperty>>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            filters.Add(fb.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));

        if (!string.IsNullOrWhiteSpace(filter.Address))
            filters.Add(fb.Regex(p => p.AddressProperty, new BsonRegularExpression(filter.Address, "i")));

        if (filter.MinPrice.HasValue)
            filters.Add(fb.Gte(p => p.PriceProperty, filter.MinPrice.Value));

        if (filter.MaxPrice.HasValue)
            filters.Add(fb.Lte(p => p.PriceProperty, filter.MaxPrice.Value));

        // Solo disponibles
        filters.Add(fb.Eq(p => p.Status, "Available"));

        return filters.Any() ? fb.And(filters) : fb.Empty;
    }

}