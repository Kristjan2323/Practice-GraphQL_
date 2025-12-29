using graphql.DataLoaders;
using graphql.Models;

namespace graphql.Services;

// TYPE CONFIGURATION: Tells HotChocolate how to resolve Book fields
// This is registered in Program.cs with .AddType<BookType>()
public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        // Simple fields - no custom resolver needed
        // HotChocolate automatically maps these from Book properties
        descriptor
        .Field(b => b.Id)
        .Type<NonNullType<IdType>>();

        descriptor
        .Field(b => b.Title)
        .Type<NonNullType<StringType>>();

        descriptor
        .Field(b => b.Pages)
        .Type<IntType>();

        // CUSTOM FIELD: "author" doesn't exist on Book model
        // Book only has "AuthorId" (string), not "Author" (object)
        // So we tell HotChocolate: "When 'author' is requested, call GetAuthorAsync"
        descriptor
        .Field("author")  // GraphQL field name
        .Type<AuthorType>()  // Returns an Author object
        .ResolveWith<Resolvers>(r => r.GetAuthorAsync(default!, default!, default));
        // This means: "Use the Resolvers.GetAuthorAsync method to get the author"
    }


}


// RESOLVER CLASS: Contains methods that fetch related data
public class Resolvers
{
    // STEP 4: This method is called for EACH book that needs its author
    // Called 5 times (once per book), but DataLoader batches the database calls!
    //
    // Execution order:
    // 1st call: GetAuthorAsync(Book1, dataLoader) → Queues authorId "a1"
    // 2nd call: GetAuthorAsync(Book2, dataLoader) → Queues authorId "a2"
    // 3rd call: GetAuthorAsync(Book3, dataLoader) → Queues authorId "a3"
    // 4th call: GetAuthorAsync(Book4, dataLoader) → Queues authorId "a4"
    // 5th call: GetAuthorAsync(Book5, dataLoader) → Queues authorId "a5"
    // Then: DataLoader batches all 5 IDs and calls LoadBatchAsync ONCE
    public async Task<Author?> GetAuthorAsync(
        [Parent] Book book,              // The current book being processed
        AuthorByIdDataLoader dataLoader,  // HotChocolate injects this (registered in Program.cs)
        CancellationToken ct)
    {
        Console.WriteLine($"🔍 GetAuthorAsync called for book: {book.Title}, AuthorId: {book.AuthorId}");

        // STEP 5: Validate - if no authorId, return null
        if (string.IsNullOrEmpty(book.AuthorId))
            return null;

        // STEP 6: Queue this authorId for batching
        // DataLoader.LoadAsync DOES NOT execute immediately!
        // It queues the request and waits to see if more requests come in
        // After all 5 books call LoadAsync, DataLoader batches them together
        // and calls LoadBatchAsync (see AuthorByIdDataLoader.cs)
        return await dataLoader.LoadAsync(book.AuthorId, ct);

        // NOTE: The 'await' here waits for the BATCH to complete, not individual queries
        // So all 5 GetAuthorAsync calls wait together, then all get results at once
    }
}
