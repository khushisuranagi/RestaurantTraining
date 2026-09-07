using MediatR;
using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.CreateLessonResource
{
    public class CreateLessonResourceCommand
        : IRequest<CreateLessonResourceResponse>
    {
        public int LessonId { get; set; }

        public ResourceType ResourceType { get; set; }

        public string ResourceUrl { get; set; } = string.Empty;

        public string? FileData { get; set; }

        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public string ContentText { get; set; } = string.Empty;

        public int SortOrder { get; set; }
    }
}