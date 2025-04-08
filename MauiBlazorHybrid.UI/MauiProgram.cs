using System.Reflection;
using CommunityToolkit.Datasync.Client.Http;
using MauiBlazorHybrid.UI.Configuration;
using MauiBlazorHybrid.UI.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;

namespace MauiBlazorHybrid.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        Assembly a = Assembly.GetExecutingAssembly();
        string appSettings = $"{a.GetName().Name}.appsettings.json";
        using Stream? stream = a.GetManifestResourceStream(appSettings);
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonStream(stream)
#if DEBUG
            .AddUserSecrets<MauiApp>()
#endif
            .Build();
        builder.Configuration.AddConfiguration(config);

        builder.Services.AddMudServices(config =>
        {
            // SnackBar Configuration
            // NOTE: did not find a global option to turn off the Icon, but you can do that in the individual Snackbar call :(
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000; // 5 seconds (default)
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        SqliteConnection dbConnection = new SqliteConnection("Data Source=MauiBlazorHybrid.db");

        builder.Services.Configure<ApiConfig>(builder.Configuration.GetSection("Api"));
        ConfigureHttpClientFactory(builder);
        builder.Services.AddScoped<IDbInitializer, DbContextInitializer>()
            .AddDbContext<AppDbContext>(options => options.UseSqlite(dbConnection));

        var app = builder.Build();

        // Initialize the database using the registered database initializer.
        InitializeDatabase(app.Services);

        return app;
    }

    private static void InitializeDatabase(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();
        IDbInitializer initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        initializer.Initialize();
    }

    private static void ConfigureHttpClientFactory(MauiAppBuilder builder)
    {
        ApiConfig? apiConfig = builder.Configuration.GetSection("Api").Get<ApiConfig>();
        HttpClientOptions options = new()
        {
            Endpoint = new Uri(apiConfig!.BaseUrl),
            // HttpPipeline = [
            //     new LoggingHandler(),
            //     new AuthenticationHandler()
            // ],
            Timeout = TimeSpan.FromSeconds(120),
            UserAgent = "mocella/datasync-mauiblazorhybrid-service"
        };

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
    }
}
