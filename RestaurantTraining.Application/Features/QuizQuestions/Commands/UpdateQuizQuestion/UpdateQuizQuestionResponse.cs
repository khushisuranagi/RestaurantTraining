using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizQuestions.Commands.UpdateQuizQuestion
{
    public class UpdateQuizQuestionResponse : BaseResponse
    {
        public int QuestionId { get; set; }
    }
}