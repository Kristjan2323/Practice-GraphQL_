using graphql.Models;
using graphql.Services;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.Field(a => a.Id).Type<NonNullType<IdType>>();
        descriptor.Field(a => a.Name).Type<NonNullType<StringType>>();

        // Add books field
        descriptor
            .Field("books")
            .Type<ListType<BookType>>()
            .ResolveWith<Resolvers>(r => r.GetBooksAsync(default!, default!, default));
    }

    private class Resolvers
    {
        public async Task<List<Book>> GetBooksAsync(
            [Parent] Author author,
            BookService bookService,  // Can use service directly (not many books per author usually)
            CancellationToken ct)
        {
            return await bookService.GetByAuthorIdAsync(author.Id!);
        }
    }
}
