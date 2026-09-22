using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ResourceSummaries.Commands.GetOrGenerateResourceSummary
{
    public class GetOrGenerateResourceSummaryCommandHandler
        : IRequestHandler<GetOrGenerateResourceSummaryCommand, ResourceSummaryResult>
    {
        private readonly IResourceSummaryRepository _repository;
        private readonly IAiSummarizer _summarizer;

        public GetOrGenerateResourceSummaryCommandHandler(
            IResourceSummaryRepository repository,
            IAiSummarizer summarizer)
        {
            _repository = repository;
            _summarizer = summarizer;
        }

        public async Task<ResourceSummaryResult> Handle(
            GetOrGenerateResourceSummaryCommand request,
            CancellationToken cancellationToken)
        {
            // 1) Already summarized? Return the stored one.
            var existing = await _repository.GetSummaryAsync(
                request.ResourceId, cancellationToken);

            if (!string.IsNullOrWhiteSpace(existing))
                return new ResourceSummaryResult { SummaryText = existing };

            // 2) Gather the resource's full context (including the file data).
            var context = await _repository.GetMediaContextAsync(
                request.ResourceId, cancellationToken);

            if (context is null)
                return new ResourceSummaryResult { NotAvailable = true };

            // 3) Summarize the real content.
            var summary = await _summarizer.SummarizeAsync(new ResourceSummaryInput
            {
                LessonTitle = context.LessonTitle,
                LessonDescription = context.LessonDescription,
                ResourceType = context.ResourceType,
                ResourceUrl = context.ResourceUrl,
                ContentText = context.ContentText,
                FileName = context.FileName,
                FileData = context.FileData,
                ContentType = context.ContentType
            }, cancellationToken);

            if (string.IsNullOrWhiteSpace(summary))
                return new ResourceSummaryResult { NotAvailable = true };

            // 4) Store it so it's reused next time.
            await _repository.SaveSummaryAsync(request.ResourceId, summary, cancellationToken);

            return new ResourceSummaryResult { SummaryText = summary };
        }
    }
}
