using graphql.Models;
using graphql.Services;

namespace graphql.DataLoaders;

public class AuthorByIdDataLoader : BatchDataLoader<string, Author>
{
    private readonly AuthorService _authorService;

    public AuthorByIdDataLoader(IBatchScheduler batchScheduler,
    AuthorService authorService,
    DataLoaderOptions? dataLoaderOptions = null) : base(batchScheduler, dataLoaderOptions)
    {
        _authorService = authorService;
    }
    
    /// <summary>
    /// This method get as params all the authorIds that are unique and filter all the authors based on that
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<IReadOnlyDictionary<string, Author>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        var authors = await _authorService.GetAllAsync();
        return authors.ToDictionary(a => a.Id);
    }
}