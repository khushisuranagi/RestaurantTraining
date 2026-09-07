using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.UpdateLessonResource
{
    public class UpdateLessonResourceResponse : BaseResponse
    {
        public int ResourceId { get; set; }
    }
}