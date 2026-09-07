using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.UpdateQuizOption
{
    public class UpdateQuizOptionResponse : BaseResponse
    {
        public int OptionId { get; set; }
    }
}