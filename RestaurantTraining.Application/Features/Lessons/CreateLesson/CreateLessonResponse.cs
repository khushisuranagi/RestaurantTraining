using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Lessons.Commands.CreateLesson
{
    public class CreateLessonResponse : BaseResponse
    {
        public int LessonId { get; set; }
    }
}