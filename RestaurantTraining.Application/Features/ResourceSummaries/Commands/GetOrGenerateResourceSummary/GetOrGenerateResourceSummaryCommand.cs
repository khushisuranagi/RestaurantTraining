using MediatR;

namespace RestaurantTraining.Application.Features.ResourceSummaries.Commands.GetOrGenerateResourceSummary
{
    // "Give me the summary for this resource — generate + store it if it doesn't
    // exist yet, otherwise return the stored one."
    public class GetOrGenerateResourceSummaryCommand : IRequest<ResourceSummaryResult>
    {
        public int ResourceId { get; set; }
    }

    public class ResourceSummaryResult
    {
        public bool NotAvailable { get; set; }   // → controller returns 404
        public string SummaryText { get; set; } = string.Empty;
    }
}
