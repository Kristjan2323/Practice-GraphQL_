using graphql.Enums;
using graphql.Models;
using graphql.Services;

namespace graphql.Schema.Query;

public class Query
{
    public async Task<List<Book>> GetBooks([Service] BookService bookService)
    {
        return await bookService.GetAllAsync();
    }

    public async Task<Book?> GetBook(string id, [Service] BookService bookService)
    {
        return await bookService.GetByIdAsync(id);
    }

    public async Task<List<Author>> GetAuthors([Service] AuthorService authorService)
    {
        return await authorService.GetAllAsync();
    }

    public async Task<Author?> GetAuthor(string id, [Service] AuthorService authorService)
    {
        return await authorService.GetByIdAsync(id);
    }

    public async Task<List<Book>> GetBooksByAuthor(string authorId, [Service] BookService bookService)
    {
        return await bookService.GetByAuthorIdAsync(authorId);
    }

    public Roles GetRoles(Roles role) => role;
}