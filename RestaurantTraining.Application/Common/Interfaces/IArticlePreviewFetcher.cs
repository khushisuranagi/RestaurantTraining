namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface IArticlePreviewFetcher
    {
        Task<ArticlePreviewResult> FetchAsync(string url, CancellationToken cancellationToken);
    }

    public class ArticlePreviewResult
    {
        public bool Success { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
    }
}