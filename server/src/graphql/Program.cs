using graphql.Configuration;
using graphql.DataLoaders;
using graphql.Schema.Mutations;
using graphql.Schema.Query;
using graphql.Schema.Subscription;
using graphql.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register MongoDB services
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<AuthorService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// GRAPHQL CONFIGURATION: Wire everything together
builder.Services
    .AddGraphQLServer()
    .AddDefaultTransactionScopeHandler()

    // Register root Query type - contains GetBooks(), GetAuthors(), etc.
    .AddQueryType<Query>()

    .AddMutationType<Mutation>()
    .AddMutationConventions()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions()

    // Register custom type configurations
    // This tells HotChocolate: "Use BookType to configure how Book fields are resolved"
    .AddType<BookType>()   // Registers the "author" field resolver
    .AddType<AuthorType>()  // Registers the "books" field resolver

    // Register DataLoader for dependency injection
    // When BookType resolver requests AuthorByIdDataLoader, HotChocolate provides it
    // DataLoaders are SCOPED per request - each GraphQL query gets a fresh instance
    .AddDataLoader<AuthorByIdDataLoader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseWebSockets();

app.MapGraphQL();

app.UseHttpsRedirection();

app.Run();

