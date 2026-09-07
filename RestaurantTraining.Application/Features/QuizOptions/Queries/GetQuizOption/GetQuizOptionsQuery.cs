using MediatR;

namespace RestaurantTraining.Application.Features.QuizOptions.Queries.GetQuizOptions
{
    public class GetQuizOptionsQuery : IRequest<List<QuizOptionDto>>
    {
    }
}