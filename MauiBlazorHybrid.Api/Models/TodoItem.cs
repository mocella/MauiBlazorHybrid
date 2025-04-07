using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;

namespace MauiBlazorHybrid.Api.Models;

public class TodoItem : EntityTableData
{
    [Required]
    [MinLength(1)]
    public string Title { get; set; } = string.Empty;

    public bool IsComplete { get; set; }
}
