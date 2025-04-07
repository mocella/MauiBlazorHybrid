using Microsoft.AspNetCore.Cors.Infrastructure;

namespace MauiBlazorHybrid.Api.Configuration;

public static class CorsSettings
{
    public static Action<CorsOptions> SetupCorsOptions() =>
        options =>
        {
            options.AddPolicy("AllowLocalHost",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Chat-Message-Id")
            );
        };
}
