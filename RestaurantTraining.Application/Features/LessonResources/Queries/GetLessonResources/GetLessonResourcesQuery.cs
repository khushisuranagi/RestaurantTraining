using MediatR;

namespace RestaurantTraining.Application.Features.LessonResources.Queries.GetLessonResources
{
    public class GetLessonResourcesQuery : IRequest<List<LessonResourceDto>>
    {
    }
}