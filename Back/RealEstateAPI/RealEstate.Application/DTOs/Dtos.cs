namespace RealEstate.Application.DTOs;

public record PropertyDto(
    string Id,
    string IdOwner,
    string Name,
    string AddressProperty,
    double PriceProperty,
    string Image
);

public record PropertyDetailDto(
    string Id,
    string IdOwner,
    string Name,
    string AddressProperty,
    double PriceProperty,
    string Image,
    string PropertyType,
    string Status,
    int Bedrooms,
    int Bathrooms,
    double Area,
    int YearBuilt,
    string Description,
    List<string> Features,
    int ParkingSpots
);

public record PropertyFilterDto(
    string? Name,
    string? Address,
    double? MinPrice,
    double? MaxPrice,
    int PageNumber = 1,
    int PageSize = 10
);

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message = null,
    List<string>? Errors = null
);

public record PagedResult<T>(
    IEnumerable<T> Data,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);