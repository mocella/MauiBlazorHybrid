using CommunityToolkit.Datasync.Client;

namespace MauiBlazorHybrid.UI.Extensions;

public static class ServiceExtensions
{
    public static TEntity ReturnOrThrow<TEntity>(this ServiceResponse<TEntity> response)
        => response is { IsSuccessful: true, HasValue: true }
            ? response.Value!
            : throw new ApplicationException(response.ReasonPhrase);
}
