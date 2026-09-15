using MediatR;

namespace RestaurantTraining.Application.Features.ArticlePreview.Queries.GetArticlePreview
{
    public class GetArticlePreviewQuery : IRequest<ArticlePreviewDto>
    {
        public string Url { get; set; } = string.Empty;
    }

    public class ArticlePreviewDto
    {
        public bool Success { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
    }
}