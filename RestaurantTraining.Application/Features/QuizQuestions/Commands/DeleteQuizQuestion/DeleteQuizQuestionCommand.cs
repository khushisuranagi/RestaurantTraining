using MediatR;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizQuestions.Commands.DeleteQuizQuestion
{
    public class DeleteQuizQuestionCommand : IRequest<BaseResponse>
    {
        public int QuestionId { get; set; }
    }
}