using graphql.Enums;
using graphql.Models;
using graphql.Services;

namespace graphql.Schema.Query;

public class Query(BookService bookService, AuthorService authorService)
{
    public async Task<List<Book>> GetBooks()
    {
        return await bookService.GetAllAsync();
    }

    public async Task<Book?> GetBook(string id)
    {
        return await bookService.GetByIdAsync(id);
    }

    public async Task<List<Author>> GetAuthors()
    {
        return await authorService.GetAllAsync();
    }

    public async Task<Author?> GetAuthor(string id)
    {
        return await authorService.GetByIdAsync(id);
    }

    public async Task<List<Book>> GetBooksByAuthor(string authorId)
    {
        return await bookService.GetByAuthorIdAsync(authorId);
    }

    public Roles GetRoles(Roles role) => role;
}