using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RealEstate.API.Controllers;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

namespace RealEstate.Tests.API.Controllers;

[TestFixture]
public class PropertiesControllerTests
{
    private Mock<IPropertyService> _mockPropertyService;
    private Mock<IValidator<PropertyFilterDto>> _mockValidator;
    private Mock<ILogger<PropertiesController>> _mockLogger;
    private PropertiesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockPropertyService = new Mock<IPropertyService>();
        _mockValidator = new Mock<IValidator<PropertyFilterDto>>();
        _mockLogger = new Mock<ILogger<PropertiesController>>();

        _controller = new PropertiesController(
            _mockPropertyService.Object,
            _mockValidator.Object,
            _mockLogger.Object);
    }

    [Test]
    public async Task GetProperties_WithValidParameters_ShouldReturnOkResult()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var serviceResponse = new ApiResponse<PagedResult<PropertyDto>>(
            Success: true,
            Data: CreateMockPagedResult(),
            Message: "Success"
        );

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<PropertyFilterDto>(), default))
            .ReturnsAsync(validationResult);

        _mockPropertyService.Setup(s => s.GetPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _controller.GetProperties();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var response = okResult.Value as ApiResponse<PagedResult<PropertyDto>>;
        response!.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
    }

    [Test]
    public async Task GetProperties_WithInvalidParameters_ShouldReturnBadRequest()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new("PageNumber", "Page number must be greater than 0")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<PropertyFilterDto>(), default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.GetProperties(pageNumber: -1);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var response = badRequestResult.Value as ApiResponse<PagedResult<PropertyDto>>;
        response!.Success.Should().BeFalse();
        response.Errors.Should().Contain("Page number must be greater than 0");
    }

    [Test]
    public async Task GetProperties_WhenServiceFails_ShouldReturnInternalServerError()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var serviceResponse = new ApiResponse<PagedResult<PropertyDto>>(
            Success: false,
            Data: null,
            Message: "Database error"
        );

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<PropertyFilterDto>(), default))
            .ReturnsAsync(validationResult);

        _mockPropertyService.Setup(s => s.GetPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _controller.GetProperties();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Test]
    public async Task GetPropertyById_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var propertyId = "689bb0e63f15768dbc29b336";
        var serviceResponse = new ApiResponse<PropertyDetailDto?>(
            Success: true,
            Data: CreateMockPropertyDetail(),
            Message: "Success"
        );

        _mockPropertyService.Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _controller.GetPropertyById(propertyId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var response = okResult.Value as ApiResponse<PropertyDetailDto?>;
        response!.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().Be(propertyId);
    }

    [Test]
    public async Task GetPropertyById_WithEmptyId_ShouldReturnBadRequest()
    {
        // Act
        var result = await _controller.GetPropertyById("");

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var response = badRequestResult.Value as ApiResponse<PropertyDetailDto?>;
        response!.Success.Should().BeFalse();
        response.Message.Should().Be("Property ID is required");
    }

    [Test]
    public async Task GetPropertyById_WhenPropertyNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var propertyId = "nonexistent";
        var serviceResponse = new ApiResponse<PropertyDetailDto?>(
            Success: false,
            Data: null,
            Message: "Property not found"
        );

        _mockPropertyService.Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _controller.GetPropertyById(propertyId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Test]
    public void HealthCheck_ShouldReturnOkWithStatus()
    {
        // Act
        var result = _controller.HealthCheck();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var response = okResult.Value;
        response.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WithNullPropertyService_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new PropertiesController(null!, _mockValidator.Object, _mockLogger.Object));
        exception.ParamName.Should().Be("propertyService");
    }

    [Test]
    public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new PropertiesController(_mockPropertyService.Object, null!, _mockLogger.Object));
        exception.ParamName.Should().Be("filterValidator");
    }

    [Test]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new PropertiesController(_mockPropertyService.Object, _mockValidator.Object, null!));
        exception.ParamName.Should().Be("logger");
    }

    private static PagedResult<PropertyDto> CreateMockPagedResult()
    {
        var properties = new List<PropertyDto>
        {
            new("689bb0e63f15768dbc29b336", "OWN001", "Casa Moderna de Lujo La Calera",
                "Calle 15 #23-45, Conjunto Reservado, La Calera, Cundinamarca",
                850000000, "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&h=600&fit=crop")
        };

        return new PagedResult<PropertyDto>(properties, 1, 1, 10, 1);
    }

    private static PropertyDetailDto CreateMockPropertyDetail()
    {
        return new PropertyDetailDto(
            "689bb0e63f15768dbc29b336",
            "OWN001",
            "Casa Moderna de Lujo La Calera",
            "Calle 15 #23-45, Conjunto Reservado, La Calera, Cundinamarca",
            850000000,
            "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&h=600&fit=crop",
            "Casa",
            "Available",
            4,
            3,
            280.5,
            2020,
            "Hermosa casa moderna con acabados de lujo, vista panorámica de la sabana de Bogotá.",
            new List<string> { "Piscina", "Jardín privado", "Chimenea", "Estudio", "Cuarto de servicio" },
            2
        );
    }
}