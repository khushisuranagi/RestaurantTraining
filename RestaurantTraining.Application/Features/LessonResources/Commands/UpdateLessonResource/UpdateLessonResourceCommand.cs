using MediatR;
using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.UpdateLessonResource
{
    public class UpdateLessonResourceCommand
        : IRequest<UpdateLessonResourceResponse>
    {
        public int ResourceId { get; set; }

        public int LessonId { get; set; }

        public ResourceType ResourceType { get; set; }

        public string ResourceUrl { get; set; } = string.Empty;

        public string? FileData { get; set; }

        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public string ContentText { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}