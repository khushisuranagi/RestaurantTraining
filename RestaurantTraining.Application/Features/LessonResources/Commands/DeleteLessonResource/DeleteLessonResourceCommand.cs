using MediatR;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.LessonResources.Commands.DeleteLessonResource
{
    public class DeleteLessonResourceCommand : IRequest<BaseResponse>
    {
        public int ResourceId { get; set; }
    }
}