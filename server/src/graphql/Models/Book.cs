using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace graphql.Models;

public record Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }
    
    public string Title { get; init; } = string.Empty;

    public int Pages { get; init; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? AuthorId { get; init; }
}