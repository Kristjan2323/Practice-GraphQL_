using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace graphql.Models;

public record Author
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    public string Name { get; init; } = string.Empty;
}