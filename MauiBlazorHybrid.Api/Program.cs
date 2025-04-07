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
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalHost");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
