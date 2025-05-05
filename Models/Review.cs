using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcommerceApp.Models;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string OrderId { get; set; }
    public string SellerId { get; set; }
    public string BuyerId { get; set; }
    public int Rating { get; set; } // 1–5
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}