using MediatR;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.DeleteQuizOption
{
    public class DeleteQuizOptionCommand : IRequest<BaseResponse>
    {
        public int OptionId { get; set; }
    }
}