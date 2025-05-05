using EcommerceApp.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcommerceApp.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string ListingId { get; set; }
    public string BuyerId { get; set; }
    public string SellerId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;

    [BsonRepresentation(BsonType.String)]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}