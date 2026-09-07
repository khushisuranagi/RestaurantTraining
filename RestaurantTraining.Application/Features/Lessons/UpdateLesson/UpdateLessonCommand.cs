using MediatR;


namespace RestaurantTraining.Application.Features.Lessons.Commands.UpdateLesson
{
    public class UpdateLessonCommand : IRequest<UpdateLessonResponse>
    {
        public int LessonId { get; set; }

        public int ModuleId { get; set; }

        public string LessonTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}