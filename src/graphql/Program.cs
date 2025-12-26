using graphql.Configuration;
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
builder.Services
    .AddGraphQLServer()
    .AddDefaultTransactionScopeHandler()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMutationConventions()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

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

