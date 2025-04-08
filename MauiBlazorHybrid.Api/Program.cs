using CommunityToolkit.Datasync.Server;
using CommunityToolkit.Datasync.Server.Swashbuckle;
using MauiBlazorHybrid.Api.Configuration;
using MauiBlazorHybrid.Api.MIddleware;
using MauiBlazorHybrid.Api.Models;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(CorsSettings.SetupCorsOptions());
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ApplicationException("DefaultConnection is not set");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatasyncServices();

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options => options.AddDatasyncControllers());

WebApplication app = builder.Build();


// Initialize the database
using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.InitializeDatabaseAsync();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalHost");
}

app.UseAuthorization();
app.MapControllers();

app.Run();
