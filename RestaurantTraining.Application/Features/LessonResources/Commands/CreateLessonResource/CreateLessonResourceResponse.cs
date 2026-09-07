using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.CreateLessonResource
{
    public class CreateLessonResourceResponse : BaseResponse
    {
        public int ResourceId { get; set; }
    }
}