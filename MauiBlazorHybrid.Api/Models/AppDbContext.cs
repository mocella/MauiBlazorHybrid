using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MauiBlazorHybrid.Api.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public async Task InitializeDatabaseAsync()
    {
        _ = await Database.EnsureCreatedAsync();

        // TODO: not sure how I feel about triggers when using EF Core - AuditInterceptor would be better....
        const string datasyncTrigger = @"
            CREATE OR ALTER TRIGGER [dbo].[{0}_datasync] ON [dbo].[{0}] AFTER INSERT, UPDATE AS
            BEGIN
                SET NOCOUNT ON;
                UPDATE
                    [dbo].[{0}]
                SET
                    [UpdatedAt] = SYSUTCDATETIME()
                WHERE
                    [Id] IN (SELECT [Id] FROM INSERTED);
            END
        "
            ;

        // Install the above trigger to set the UpdatedAt field automatically on insert or update.
        foreach (IEntityType table in Model.GetEntityTypes())
        {
            string sql = string.Format(datasyncTrigger, table.GetTableName());
            _ = await Database.ExecuteSqlRawAsync(sql);
        }
    }

    [SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "Model builder ignores return value.")]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: again, not sure Triggers are the way, but example app uses them.
        modelBuilder.Entity<TodoItem>()
            .ToTable(tb => tb.HasTrigger("TodoItem_datasync"));

        base.OnModelCreating(modelBuilder);
    }
}
