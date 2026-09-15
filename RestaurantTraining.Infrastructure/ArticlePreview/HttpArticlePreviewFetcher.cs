using AngleSharp;
using AngleSharp.Dom;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Infrastructure.ArticlePreview
{
    public class HttpArticlePreviewFetcher : IArticlePreviewFetcher
    {
        private readonly HttpClient _httpClient;

        public HttpArticlePreviewFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(6);
        }

        public async Task<ArticlePreviewResult> FetchAsync(
            string url, CancellationToken cancellationToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd(
                    "Mozilla/5.0 (compatible; RestaurantTrainingBot/1.0)");

                var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return new ArticlePreviewResult { Success = false };

                var html = await response.Content.ReadAsStringAsync(cancellationToken);

                var config = Configuration.Default;
                using var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(
                    req => req.Content(html), cancellationToken);

                string GetMeta(string property) =>
                    document.QuerySelector($"meta[property='{property}']")
                        ?.GetAttribute("content")
                    ?? document.QuerySelector($"meta[name='{property}']")
                        ?.GetAttribute("content")
                    ?? string.Empty;

                var title = GetMeta("og:title");
                if (string.IsNullOrWhiteSpace(title))
                    title = document.QuerySelector("title")?.TextContent ?? string.Empty;

                return new ArticlePreviewResult
                {
                    Success = true,
                    Title = title.Trim(),
                    Description = GetMeta("og:description") is { Length: > 0 } d
                        ? d : GetMeta("description"),
                    ImageUrl = GetMeta("og:image"),
                    SiteName = GetMeta("og:site_name")
                };
            }
            catch
            {
                // Any failure (timeout, blocked, malformed HTML) -> just fall back to a plain link.
                return new ArticlePreviewResult { Success = false };
            }
        }
    }
}