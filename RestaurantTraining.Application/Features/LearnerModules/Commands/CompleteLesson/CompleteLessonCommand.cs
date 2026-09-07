using MediatR;

namespace RestaurantTraining.Application.Features.LearnerModules.Commands.CompleteLesson
{
    public class CompleteLessonCommand : IRequest<CompleteLessonResultDto>
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
    }
}
