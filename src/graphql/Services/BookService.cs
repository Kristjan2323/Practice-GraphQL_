using graphql.Configuration;
using graphql.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace graphql.Services;

public class BookService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BookService(IOptions<MongoDBSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _booksCollection = mongoDatabase.GetCollection<Book>(mongoDbSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetAllAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetByIdAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<Book>> GetByAuthorIdAsync(string authorId) =>
        await _booksCollection.Find(x => x.AuthorId == authorId).ToListAsync();

    public async Task CreateAsync(Book book) =>
        await _booksCollection.InsertOneAsync(book);

    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task DeleteAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}
