using graphql.Models;
using graphql.Schema.Mutations;

namespace graphql.Schema.Subscription;

public class Subscription
{
    [Subscribe]
    [Topic(nameof(Mutation.PublishBook))]
    public BookResults OnBookPublished([EventMessage] BookResults book) => book;
}