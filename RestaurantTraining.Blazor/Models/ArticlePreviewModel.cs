namespace RestaurantTraining.Blazor.Models;

public class ArticlePreviewModel
{
    public bool Success { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
}