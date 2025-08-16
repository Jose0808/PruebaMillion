using FluentAssertions;
using Moq;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;

namespace RealEstate.Tests.Application.Services;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _mockRepository;
    private PropertyService _propertyService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPropertyRepository>();
        _propertyService = new PropertyService(_mockRepository.Object);
    }

    [Test]
    public async Task GetPropertiesAsync_WithValidFilter_ShouldReturnPagedResult()
    {
        // Arrange
        var properties = CreateMockProperties();
        var filter = new PropertyFilterDto("Casa", null, null, null, 1, 10);

        _mockRepository.Setup(r => r.GetByFiltersAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync(properties);

        // Act
        var result = await _propertyService.GetPropertiesAsync(filter);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Data.Should().HaveCount(2);
        result.Message.Should().Be("Properties retrieved successfully");
    }

    [Test]
    public async Task GetPropertiesAsync_WhenRepositoryThrows_ShouldReturnErrorResponse()
    {
        // Arrange
        var filter = new PropertyFilterDto("Casa", null, null, null, 1, 10);

        _mockRepository.Setup(r => r.GetByFiltersAsync(It.IsAny<PropertyFilter>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _propertyService.GetPropertiesAsync(filter);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Be("Error retrieving properties");
        result.Errors.Should().Contain("Database error");
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithValidId_ShouldReturnProperty()
    {
        // Arrange
        var property = CreateMockProperties().First();
        var propertyId = "689bb0e63f15768dbc29b336";

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(propertyId);
        result.Data.Name.Should().Be("Casa Moderna de Lujo La Calera");
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithInvalidId_ShouldReturnErrorResponse()
    {
        // Arrange
        var invalidId = "";

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(invalidId);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Be("Property ID is required");
        result.Errors.Should().Contain("Invalid property ID");
    }

    [Test]
    public async Task GetPropertyByIdAsync_WhenPropertyNotFound_ShouldReturnNotFoundResponse()
    {
        // Arrange
        var propertyId = "nonexistent";

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.Message.Should().Be("Property not found");
    }

    [Test]
    public void PropertyService_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new PropertyService(null!));
        exception.ParamName.Should().Be("propertyRepository");
    }

    private static List<Property> CreateMockProperties()
    {
        return new List<Property>
        {
            new()
            {
                Id = "689bb0e63f15768dbc29b336",
                IdOwner = "OWN001",
                Name = "Casa Moderna de Lujo La Calera",
                AddressProperty = "Calle 15 #23-45, Conjunto Reservado, La Calera, Cundinamarca",
                PriceProperty = 850000000,
                Image = "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&h=600&fit=crop",
                PropertyType = "Casa",
                Status = "Available",
                Bedrooms = 4,
                Bathrooms = 3,
                Area = 280.5,
                YearBuilt = 2020,
                Description = "Hermosa casa moderna con acabados de lujo, vista panorámica de la sabana de Bogotá.",
                Features = new List<string> { "Piscina", "Jardín privado", "Chimenea", "Estudio", "Cuarto de servicio" },
                ParkingSpots = 2,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new()
            {
                Id = "689bb0e63f15768dbc29b337",
                IdOwner = "OWN002",
                Name = "Apartamento Ejecutivo Bogotá",
                AddressProperty = "Carrera 11 #93-45, Chapinero, Bogotá",
                PriceProperty = 450000000,
                Image = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800&h=600&fit=crop",
                PropertyType = "Apartamento",
                Status = "Available",
                Bedrooms = 2,
                Bathrooms = 2,
                Area = 85.0,
                YearBuilt = 2018,
                Description = "Moderno apartamento en el corazón de Chapinero.",
                Features = new List<string> { "Balcón", "Gimnasio", "Portería 24h" },
                ParkingSpots = 1,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };
    }
}