using graphql.Enums;
using graphql.Models;
using graphql.Services;

namespace graphql.Schema.Query;

public class Query(BookService bookService, AuthorService authorService)
{
    // STEP 1: GraphQL Execution Starts Here
    // When client requests: query { books { id title author { name } } }
    // This is the ROOT resolver - it's called FIRST
    public async Task<List<Book>> GetBooks()
    {
        // STEP 2: Fetch all books from database (1 query)
        // Returns: List of 5 Book objects with their AuthorId properties
        // At this point, NO authors are fetched yet!
        // Example return: [
        //   { Id: "1", Title: "Book1", AuthorId: "a1" },
        //   { Id: "2", Title: "Book2", AuthorId: "a2" },
        //   ... (5 books total)
        // ]
        return await bookService.GetAllAsync();

        // STEP 3: HotChocolate now processes each book individually
        // For EACH book, it checks: "Did the client request the 'author' field?"
        // If YES → Go to BookType resolver (see BookType.cs)
        // If NO → Skip author resolution, return books as-is
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