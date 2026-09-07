using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Lessons.Commands.UpdateLesson
{
    public class UpdateLessonResponse : BaseResponse
    {
        public int LessonId { get; set; }
    }
}