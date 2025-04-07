namespace MauiBlazorHybrid.UI.Components.Models;

public class CatalogItem
{
    public Guid ItemId { get; set; }
    public string Name { get; set; }
    public string Dimensions { get; set; }
    public decimal Price { get; set; }
    public int InStockQuantity { get; set; }
}