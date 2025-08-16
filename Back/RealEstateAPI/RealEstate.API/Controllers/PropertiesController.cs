using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IValidator<PropertyFilterDto> _filterValidator;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(
        IPropertyService propertyService,
        IValidator<PropertyFilterDto> filterValidator,
        ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        _filterValidator = filterValidator ?? throw new ArgumentNullException(nameof(filterValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene una lista paginada de propiedades con filtros opcionales
    /// </summary>
    /// <param name="name">Filtro por nombre de la propiedad</param>
    /// <param name="address">Filtro por dirección de la propiedad</param>
    /// <param name="minPrice">Precio mínimo</param>
    /// <param name="maxPrice">Precio máximo</param>
    /// <param name="pageNumber">Número de página (por defecto 1)</param>
    /// <param name="pageSize">Tamaño de página (por defecto 10, máximo 100)</param>
    /// <returns>Lista paginada de propiedades</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PropertyDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PropertyDto>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PropertyDto>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PagedResult<PropertyDto>>>> GetProperties(
        [FromQuery] string? name = null,
        [FromQuery] string? address = null,
        [FromQuery] double? minPrice = null,
        [FromQuery] double? maxPrice = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var filter = new PropertyFilterDto(name, address, minPrice, maxPrice, pageNumber, pageSize);

            var validationResult = await _filterValidator.ValidateAsync(filter);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<PagedResult<PropertyDto>>(
                    Success: false,
                    Data: null,
                    Message: "Invalid filter parameters",
                    Errors: errors
                );

                return BadRequest(errorResponse);
            }

            _logger.LogInformation("Getting properties with filters: {@Filter}", filter);

            var result = await _propertyService.GetPropertiesAsync(filter);

            if (!result.Success)
            {
                _logger.LogWarning("Failed to get properties: {Message}", result.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting properties");
            var errorResponse = new ApiResponse<PagedResult<PropertyDto>>(
                Success: false,
                Data: null,
                Message: "An unexpected error occurred",
                Errors: new List<string> { "Internal server error" }
            );

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    /// <summary>
    /// Obtiene los detalles de una propiedad específica por su ID
    /// </summary>
    /// <param name="id">ID único de la propiedad</param>
    /// <returns>Detalles completos de la propiedad</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PropertyDetailDto?>>> GetPropertyById(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                var errorResponse = new ApiResponse<PropertyDetailDto?>(
                    Success: false,
                    Data: null,
                    Message: "Property ID is required",
                    Errors: new List<string> { "Invalid property ID" }
                );

                return BadRequest(errorResponse);
            }

            _logger.LogInformation("Getting property details for ID: {PropertyId}", id);

            var result = await _propertyService.GetPropertyByIdAsync(id);

            if (!result.Success)
            {
                if (result.Message == "Property not found")
                {
                    return NotFound(result);
                }

                _logger.LogWarning("Failed to get property {PropertyId}: {Message}", id, result.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting property {PropertyId}", id);
            var errorResponse = new ApiResponse<PropertyDetailDto?>(
                Success: false,
                Data: null,
                Message: "An unexpected error occurred",
                Errors: new List<string> { "Internal server error" }
            );

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    /// <summary>
    /// Endpoint de salud para verificar el estado de la API
    /// </summary>
    /// <returns>Estado de salud de la API</returns>
    [HttpGet("health")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult<object> HealthCheck()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }
}