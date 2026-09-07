using MediatR;
using RestaurantTraining.Application.Features.Lessons.Queries.GetLessons;

namespace RestaurantTraining.Application.Features.Lessons.Queries.GetLessons
{
    public class GetLessonsQuery : IRequest<List<LessonDto>>
    {
    }
}