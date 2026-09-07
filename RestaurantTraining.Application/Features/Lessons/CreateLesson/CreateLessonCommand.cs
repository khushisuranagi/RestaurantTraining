using MediatR;

namespace RestaurantTraining.Application.Features.Lessons.Commands.CreateLesson
{
    public class CreateLessonCommand : IRequest<CreateLessonResponse>
    {
        public int ModuleId { get; set; }  //module the lesson belongs to

        public string LessonTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int SortOrder { get; set; }  //position of lesson within the module
    }
}