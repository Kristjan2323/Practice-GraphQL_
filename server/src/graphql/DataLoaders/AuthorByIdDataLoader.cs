using graphql.Models;
using graphql.Services;

namespace graphql.DataLoaders;

// DATALOADER: Batches multiple individual requests into a single database call
// Inherits from BatchDataLoader<TKey, TValue>
//   TKey = string (the authorId we're looking up)
//   TValue = Author (the object we want to return)
public class AuthorByIdDataLoader : BatchDataLoader<string, Author>
{
    private readonly AuthorService _authorService;

    // CONSTRUCTOR: HotChocolate calls this when creating the DataLoader
    // IBatchScheduler: Manages when batching happens (provided by HotChocolate)
    public AuthorByIdDataLoader(IBatchScheduler batchScheduler,
    AuthorService authorService,
    DataLoaderOptions? dataLoaderOptions = null) : base(batchScheduler, dataLoaderOptions)
    {
        _authorService = authorService;
    }
    
    // STEP 7: THE BATCHING MAGIC HAPPENS HERE!
    // This method is called ONCE with ALL the authorIds that were queued
    //
    // Example: If 5 books requested their authors:
    //   keys = ["a1", "a2", "a3", "a4", "a5"]
    //
    // WITHOUT DataLoader:
    //   - 5 separate database queries (N+1 problem)
    //   - Query 1: SELECT * FROM Authors WHERE Id = 'a1'
    //   - Query 2: SELECT * FROM Authors WHERE Id = 'a2'
    //   - Query 3: SELECT * FROM Authors WHERE Id = 'a3'
    //   - Query 4: SELECT * FROM Authors WHERE Id = 'a4'
    //   - Query 5: SELECT * FROM Authors WHERE Id = 'a5'
    //
    // WITH DataLoader:
    //   - 1 database query (or optimally: SELECT * FROM Authors WHERE Id IN ('a1','a2','a3','a4','a5'))
    //   - This method is called ONCE with all 5 IDs
    protected override async Task<IReadOnlyDictionary<string, Author>> LoadBatchAsync(
        IReadOnlyList<string> keys,  // All the authorIds that were requested (e.g., ["a1", "a2", "a3", "a4", "a5"])
        CancellationToken cancellationToken)
    {
        // STEP 8: Fetch authors from database
        // TODO: Optimize this! Currently fetches ALL authors (inefficient)
        // Better: Implement GetByIdsAsync(keys) to only fetch requested authors
        // e.g., SELECT * FROM Authors WHERE Id IN (@keys)
        var authors = await _authorService.GetAllAsync();

        // STEP 9: Convert to dictionary for DataLoader to map back to requests
        // DataLoader expects: Dictionary<authorId, Author>
        // Example result: {
        //   "a1" => Author { Id: "a1", Name: "Author 1" },
        //   "a2" => Author { Id: "a2", Name: "Author 2" },
        //   ...
        // }
        return authors.ToDictionary(a => a.Id);

        // STEP 10: DataLoader automatically maps these results back
        // - Book1's GetAuthorAsync gets Author with Id "a1"
        // - Book2's GetAuthorAsync gets Author with Id "a2"
        // - Book3's GetAuthorAsync gets Author with Id "a3"
        // - Book4's GetAuthorAsync gets Author with Id "a4"
        // - Book5's GetAuthorAsync gets Author with Id "a5"
        //
        // STEP 11: HotChocolate assembles the final GraphQL response
        // Returns to client: {
        //   "data": {
        //     "books": [
        //       { "id": "1", "title": "Book1", "author": { "id": "a1", "name": "Author 1" } },
        //       { "id": "2", "title": "Book2", "author": { "id": "a2", "name": "Author 2" } },
        //       ... (all 5 books with their authors)
        //     ]
        //   }
        // }
    }
}