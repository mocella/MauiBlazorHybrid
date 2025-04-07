using Microsoft.EntityFrameworkCore;

namespace MauiBlazorHybrid.Api.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}
