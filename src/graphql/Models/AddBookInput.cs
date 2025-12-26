namespace graphql.Models;

public record AddBookInput(
    string Title,
    int Pages,
    string AuthorId
);