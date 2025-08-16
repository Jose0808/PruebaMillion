using System.Runtime.InteropServices.Marshalling;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Services;

public interface IPropertyService
{
    Task<ApiResponse<PagedResult<PropertyDto>>> GetPropertiesAsync(PropertyFilterDto filter);
    Task<ApiResponse<PropertyDetailDto?>> GetPropertyByIdAsync(string id);
}

public class PropertyService : IPropertyService
{
    private readonly RealEstate.Domain.Repositories.IPropertyRepository _propertyRepository;

    public PropertyService(RealEstate.Domain.Repositories.IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
    }

    public async Task<ApiResponse<PagedResult<PropertyDto>>> GetPropertiesAsync(PropertyFilterDto filterDto)
    {
        try
        {
            var filter = new RealEstate.Domain.Repositories.PropertyFilter
            {
                Name = filterDto.Name,
                Address = filterDto.Address,
                MinPrice = filterDto.MinPrice,
                MaxPrice = filterDto.MaxPrice,
                PageNumber = filterDto.PageNumber,
                PageSize = filterDto.PageSize
            };

            var (properties, totalCount, totalPages) = await _propertyRepository.GetByFiltersAsync(filter);

            var propertyDtos = properties.Select(p => new PropertyDto(
                p.Id,
                p.IdOwner,
                p.Name,
                p.AddressProperty,
                p.PriceProperty,
                p.Image
            )).ToList();


            var pagedResult = new PagedResult<PropertyDto>(
                propertyDtos,
                totalCount,
                filterDto.PageNumber,
                filterDto.PageSize,
                totalPages
            );

            return new ApiResponse<PagedResult<PropertyDto>>(
                Success: true,
                Data: pagedResult,
                Message: "Properties retrieved successfully"
            );
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResult<PropertyDto>>(
                Success: false,
                Data: null,
                Message: "Error retrieving properties",
                Errors: new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<PropertyDetailDto?>> GetPropertyByIdAsync(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new ApiResponse<PropertyDetailDto?>(
                    Success: false,
                    Data: null,
                    Message: "Property ID is required",
                    Errors: new List<string> { "Invalid property ID" }
                );
            }

            var property = await _propertyRepository.GetByIdAsync(id);

            if (property == null)
            {
                return new ApiResponse<PropertyDetailDto?>(
                    Success: false,
                    Data: null,
                    Message: "Property not found"
                );
            }

            var propertyDetailDto = new PropertyDetailDto(
                property.Id,
                property.IdOwner,
                property.Name,
                property.AddressProperty,
                property.PriceProperty,
                property.Image,
                property.PropertyType,
                property.Status,
                property.Bedrooms,
                property.Bathrooms,
                property.Area,
                property.YearBuilt,
                property.Description,
                property.Features,
                property.ParkingSpots
            );

            return new ApiResponse<PropertyDetailDto?>(
                Success: true,
                Data: propertyDetailDto,
                Message: "Property retrieved successfully"
            );
        }
        catch (Exception ex)
        {
            return new ApiResponse<PropertyDetailDto?>(
                Success: false,
                Data: null,
                Message: "Error retrieving property",
                Errors: new List<string> { ex.Message }
            );
        }
    }
}