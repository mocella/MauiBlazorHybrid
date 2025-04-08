using CommunityToolkit.Datasync.Client.Offline;
using Microsoft.EntityFrameworkCore;

namespace MauiBlazorHybrid.UI.Models;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IHttpClientFactory httpClientFactory) : OfflineDbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnDatasyncInitialization(DatasyncOfflineOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseHttpClientFactory(httpClientFactory);

        // NOTE: can customize mapping rules for each entity as needed:
        // optionsBuilder.Entity<Movie>(cfg => {
        //     cfg.ClientName = "movies";
        //     cfg.Endpoint = new Uri("/api/movies", UriKind.Relative),
        //     cfg.Query.Where(x => x.Rating != MovieRating.R)
        // });
    }

    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        PushResult pushResult = await this.PushAsync(cancellationToken);
        if (!pushResult.IsSuccessful)
        {
            throw new ApplicationException(
                $"Push failed: {pushResult.FailedRequests.FirstOrDefault().Value.ReasonPhrase}");
        }

        // NOTE: more options to configure the behavior of the push operation:
        // // Push pending operations for a subset of synchronizable entities
        // PushResult pushResult = await context.PushAsync([ typeof(Entity1), typeof(Entity2) ]);
        //
        // // Push pending operations for a single synchronizable entity.
        // PushResult pushResult = await context.Movies.PushAsync();


        PullResult pullResult = await this.PullAsync(cancellationToken);
        if (!pullResult.IsSuccessful)
        {
            throw new ApplicationException(
                $"Pull failed: {pullResult.FailedRequests.FirstOrDefault().Value.ReasonPhrase}");
        }

        // NOTE: more options to configure the behavior of the pull operation:
        // // Pull changes for a subset of synchronizable entities
        // PullResult pullResult = await context.PullAsync([ typeof(Entity1), typeof(Entity2) ]);
        //
        // // Pull changes for a single synchronizable entity.
        // PullResult pullResult = await context.Movies.PullAsync();
    }

    // NOTE: this is a custom method to resynchronize the database in a more brute-force manner (kill pending sync operations, then pull)
    public async Task ResynchronizeAsync<T>(AppDbContext context)
    {
        List<DatasyncOperation> pendingOperations = await context.DatasyncOperationsQueue.Where(x => x.EntityType == typeof(T).FullName!).ToListAsync();
        context.RemoveRange(pendingOperations);

        List<DatasyncDeltaToken> deltaTokens = await context.DatasyncDeltaTokens.Where(x => x.Id.Contains(typeof(T).FullName!)).ToListAsync();
        context.RemoveRange(deltaTokens);

        await context.SaveChangesAsync();

        PullResult pullResult = await context.PullAsync([ typeof(T) ]);
        if (!pullResult.IsSuccessful)
        {
            // Deal with any errors
        }
    }
}

/// <summary>
///     Use this class to initialize the database.  In this sample, we just create
///     the database using <see cref="DatabaseFacade.EnsureCreated" />.  However, you
///     may want to use migrations.
/// </summary>
/// <param name="context">The context for the database.</param>
public class DbContextInitializer(AppDbContext context) : IDbInitializer
{
    /// <inheritdoc />
    public void Initialize() => _ = context.Database.EnsureCreated();

    // Task.Run(async () => await context.SynchronizeAsync());
    /// <inheritdoc />
    public Task InitializeAsync(CancellationToken cancellationToken = default)
        => context.Database.EnsureCreatedAsync(cancellationToken);
}
