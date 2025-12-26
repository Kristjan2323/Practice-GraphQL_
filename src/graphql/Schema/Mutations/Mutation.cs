using graphql.Models;
using graphql.Services;
using HotChocolate.Subscriptions;

namespace graphql.Schema.Mutations;

public class Mutation
{
    public async Task<Book> AddBook(BookInput input, [Service] BookService bookService)
    {
        var book = new Book
        {
            Title = input.Title,
            Pages = input.Pages,
            AuthorId = input.AuthorId
        };

        await bookService.CreateAsync(book);
        return book;
    }

    public async Task<Book> PublishBook(
        AddBookInput input,
        [Service] BookService bookService,
        [Service] ITopicEventSender eventSender,
        CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = input.Title,
            Pages = input.Pages,
            AuthorId = input.AuthorId
        };

        await bookService.CreateAsync(book);
        await eventSender.SendAsync(nameof(PublishBook), book, cancellationToken);
        return book;
    }

    public async Task<Book> UpdateBook(string id, BookInput input, [Service] BookService bookService)
    {
        var updatedBook = new Book
        {
            Id = id,
            Title = input.Title,
            Pages = input.Pages,
            AuthorId = input.AuthorId
        };

        await bookService.UpdateAsync(id, updatedBook);
        return updatedBook;
    }

    public async Task<bool> DeleteBook(string id, [Service] BookService bookService)
    {
        await bookService.DeleteAsync(id);
        return true;
    }

    public async Task<Author> AddAuthor(string name, [Service] AuthorService authorService)
    {
        var author = new Author
        {
            Name = name
        };

        await authorService.CreateAsync(author);
        return author;
    }

    public async Task<Author> UpdateAuthor(string id, string name, [Service] AuthorService authorService)
    {
        var updatedAuthor = new Author
        {
            Id = id,
            Name = name
        };

        await authorService.UpdateAsync(id, updatedAuthor);
        return updatedAuthor;
    }

    public async Task<bool> DeleteAuthor(string id, [Service] AuthorService authorService)
    {
        await authorService.DeleteAsync(id);
        return true;
    }
}