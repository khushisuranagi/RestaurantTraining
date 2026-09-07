using MediatR;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Lessons.Commands.DeleteLesson
{
    public class DeleteLessonCommand : IRequest<BaseResponse>
    {
        public int LessonId { get; set; }
    }
}