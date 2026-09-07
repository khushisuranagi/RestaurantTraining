using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizQuestions.Commands.CreateQuizQuestion
{
    public class CreateQuizQuestionResponse : BaseResponse
    {
        public int QuestionId { get; set; }
    }
}