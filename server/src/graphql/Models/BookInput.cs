namespace graphql.Models;

public record BookInput(
    string Title,
    int Pages,
    string? AuthorId
    );