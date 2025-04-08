using System.ComponentModel.DataAnnotations;

namespace MauiBlazorHybrid.UI.Models;

public abstract class OfflineClientEntity
{
    [Key]
    public string Id { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string Version { get; set; } = Convert.ToBase64String(Array.Empty<byte>());
    public bool Deleted { get; set; }
}
