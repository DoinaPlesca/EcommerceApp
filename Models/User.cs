using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcommerceApp.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public double Rating { get; set; } = 0.0;
}