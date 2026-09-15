using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ArticlePreview.Queries.GetArticlePreview
{
    public class GetArticlePreviewQueryHandler
        : IRequestHandler<GetArticlePreviewQuery, ArticlePreviewDto>
    {
        private readonly IArticlePreviewFetcher _fetcher;

        public GetArticlePreviewQueryHandler(IArticlePreviewFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public async Task<ArticlePreviewDto> Handle(
            GetArticlePreviewQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                return new ArticlePreviewDto { Success = false };
            }

            var result = await _fetcher.FetchAsync(request.Url, cancellationToken);

            return new ArticlePreviewDto
            {
                Success = result.Success,
                Title = result.Title,
                Description = result.Description,
                ImageUrl = result.ImageUrl,
                SiteName = result.SiteName
            };
        }
    }
}