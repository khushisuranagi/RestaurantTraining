using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.CreateQuizOption
{
    public class CreateQuizOptionResponse : BaseResponse
    {
        public int OptionId { get; set; }
    }
}