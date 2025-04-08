using System.Text.Json;

namespace MauiBlazorHybrid.UI.Models;

public class TodoItem : OfflineClientEntity
{
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;

    public override string ToString()
        => JsonSerializer.Serialize(this);
}
