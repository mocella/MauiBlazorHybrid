using CommunityToolkit.Datasync.Server;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;
using MauiBlazorHybrid.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace MauiBlazorHybrid.Api.Controllers;

[Route("tables/[controller]")]
public class TodoItemController : TableController<TodoItem>
{
    public TodoItemController(AppDbContext context)
        : base(new EntityTableRepository<TodoItem>(context))
    {
    }
}
