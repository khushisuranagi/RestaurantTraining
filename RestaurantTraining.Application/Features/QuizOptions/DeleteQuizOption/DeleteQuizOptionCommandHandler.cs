using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.DeleteQuizOption
{
    public class DeleteQuizOptionCommandHandler
        : IRequestHandler<DeleteQuizOptionCommand, BaseResponse>
    {
        private readonly IQuizOptionRepository _quizOptionRepository;

        public DeleteQuizOptionCommandHandler(
            IQuizOptionRepository quizOptionRepository)
        {
            _quizOptionRepository = quizOptionRepository;
        }

        public async Task<BaseResponse> Handle(
            DeleteQuizOptionCommand request,
            CancellationToken cancellationToken)
        {
            var option =
                await _quizOptionRepository.GetQuizOptionByIdAsync(
                    request.OptionId,
                    cancellationToken);

            if (option == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Quiz option not found."
                };
            }

            await _quizOptionRepository.DeleteQuizOptionAsync(
                option,
                cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Quiz option deleted successfully."
            };
        }
    }
}