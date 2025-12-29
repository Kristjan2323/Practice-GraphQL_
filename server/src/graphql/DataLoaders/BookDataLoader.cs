using Bogus;
using graphql.Models;

namespace graphql.DataLoaders;

public static class BookDataLoader
{
    public static List<Book> GetBooks()
    {
        var bookFaker = new Faker<Book>()
            .CustomInstantiator(f => new Book
            {
                Title = f.Lorem.Sentence(),
                Pages = f.Random.Int(100, 500),
                AuthorId = null
            });

        var authorFake = new Faker<Author>()
            .CustomInstantiator(f => new Author
            {
                Name = f.Person.FullName
            });

        var books = bookFaker.Generate(5);
        var authors = authorFake.Generate(5);

        return books;
    }
}