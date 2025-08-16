using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data.Entities;

[BsonIgnoreExtraElements]
public class MongoProperty
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("IdOwner")]
    public string IdOwner { get; set; } = string.Empty;

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("AddressProperty")]
    public string AddressProperty { get; set; } = string.Empty;

    [BsonElement("PriceProperty")]
    public double PriceProperty { get; set; }

    [BsonElement("Image")]
    public string Image { get; set; } = string.Empty;

    [BsonElement("PropertyType")]
    public string PropertyType { get; set; } = string.Empty;

    [BsonElement("Status")]
    public string Status { get; set; } = string.Empty;

    [BsonElement("Bedrooms")]
    public int Bedrooms { get; set; }

    [BsonElement("Bathrooms")]
    public int Bathrooms { get; set; }

    [BsonElement("Area")]
    public double Area { get; set; }

    [BsonElement("YearBuilt")]
    public int YearBuilt { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("Features")]
    public List<string> Features { get; set; } = new();

    [BsonElement("ParkingSpots")]
    public int ParkingSpots { get; set; }

    [BsonElement("CreatedDate")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("UpdatedDate")]
    public DateTime UpdatedDate { get; set; }

    // Método para convertir de MongoProperty a Property (dominio)
    public Property ToDomainEntity()
    {
        return new Property
        {
            Id = Id,
            IdOwner = IdOwner,
            Name = Name,
            AddressProperty = AddressProperty,
            PriceProperty = PriceProperty,
            Image = Image,
            PropertyType = PropertyType,
            Status = Status,
            Bedrooms = Bedrooms,
            Bathrooms = Bathrooms,
            Area = Area,
            YearBuilt = YearBuilt,
            Description = Description,
            Features = new List<string>(Features),
            ParkingSpots = ParkingSpots,
            CreatedDate = CreatedDate,
            UpdatedDate = UpdatedDate
        };
    }

    // Método para convertir de Property (dominio) a MongoProperty
    public static MongoProperty FromDomainEntity(Property property)
    {
        return new MongoProperty
        {
            Id = property.Id,
            IdOwner = property.IdOwner,
            Name = property.Name,
            AddressProperty = property.AddressProperty,
            PriceProperty = property.PriceProperty,
            Image = property.Image,
            PropertyType = property.PropertyType,
            Status = property.Status,
            Bedrooms = property.Bedrooms,
            Bathrooms = property.Bathrooms,
            Area = property.Area,
            YearBuilt = property.YearBuilt,
            Description = property.Description,
            Features = new List<string>(property.Features),
            ParkingSpots = property.ParkingSpots,
            CreatedDate = property.CreatedDate,
            UpdatedDate = property.UpdatedDate
        };
    }
}