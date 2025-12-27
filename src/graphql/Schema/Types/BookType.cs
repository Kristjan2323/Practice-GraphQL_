using graphql.DataLoaders;
using graphql.Models;

namespace graphql.Services;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor
        .Field(b => b.Id)
        .Type<NonNullType<IdType>>();

        descriptor
        .Field(b => b.Title)
        .Type<NonNullType<StringType>>();

        descriptor
        .Field(b => b.Pages)
        .Type<IntType>();

        descriptor
        .Field("author")
        .Type<AuthorType>()
        .ResolveWith<Resolvers>(r => r.GetAuthorAsync(default!, default!, default));
    }


}


public class Resolvers
{
    // This method is called for EACH book
    // But DataLoader batches all the calls
    public async Task<Author?> GetAuthorAsync(
        [Parent] Book book,              // The current book
        AuthorByIdDataLoader dataLoader,  // Injected DataLoader
        CancellationToken ct)
    {
        Console.WriteLine($"🔍 GetAuthorAsync called for book: {book.Title}, AuthorId: {book.AuthorId}");

        if (string.IsNullOrEmpty(book.AuthorId))
            return null;

        // DataLoader batches this!
        return await dataLoader.LoadAsync(book.AuthorId, ct);
    }
}
