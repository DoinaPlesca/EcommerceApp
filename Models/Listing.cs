using EcommerceApp.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcommerceApp.Models;

public class Listing
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string SellerId { get; set; }
    public string Category { get; set; }
    public string Condition { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonRepresentation(BsonType.String)]
    public ListingStatus Status { get; set; } = ListingStatus.Available;
}