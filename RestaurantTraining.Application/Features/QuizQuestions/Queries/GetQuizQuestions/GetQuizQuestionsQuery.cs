using MediatR;

namespace RestaurantTraining.Application.Features.QuizQuestions.Queries.GetQuizQuestions
{
    public class GetQuizQuestionsQuery : IRequest<List<QuizQuestionDto>>
    {
    }
}