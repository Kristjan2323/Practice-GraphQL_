using graphql.Configuration;
using graphql.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace graphql.Services;

public class AuthorService
{
    private readonly IMongoCollection<Author> _authorsCollection;

    public AuthorService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _authorsCollection = mongoDatabase.GetCollection<Author>(mongoDBSettings.Value.AuthorsCollectionName);
    }

    public async Task<List<Author>> GetAllAsync() =>
        await _authorsCollection.Find(_ => true).ToListAsync();

    public async Task<Author?> GetByIdAsync(string id) =>
        await _authorsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Author author) =>
        await _authorsCollection.InsertOneAsync(author);

    public async Task UpdateAsync(string id, Author updatedAuthor) =>
        await _authorsCollection.ReplaceOneAsync(x => x.Id == id, updatedAuthor);

    public async Task DeleteAsync(string id) =>
        await _authorsCollection.DeleteOneAsync(x => x.Id == id);
}
