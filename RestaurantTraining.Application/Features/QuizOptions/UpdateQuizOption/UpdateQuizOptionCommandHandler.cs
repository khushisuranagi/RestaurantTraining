using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.UpdateQuizOption
{
    public class UpdateQuizOptionCommandHandler
        : IRequestHandler<UpdateQuizOptionCommand, UpdateQuizOptionResponse>
    {
        private readonly IQuizOptionRepository _quizOptionRepository;

        public UpdateQuizOptionCommandHandler(
            IQuizOptionRepository quizOptionRepository)
        {
            _quizOptionRepository = quizOptionRepository;
        }

        public async Task<UpdateQuizOptionResponse> Handle(
            UpdateQuizOptionCommand request,
            CancellationToken cancellationToken)
        {
            var option =
                await _quizOptionRepository.GetQuizOptionByIdAsync(
                    request.OptionId,
                    cancellationToken);

            if (option == null)
            {
                return new UpdateQuizOptionResponse
                {
                    Success = false,
                    Message = "Quiz option not found."
                };
            }

            if (string.IsNullOrWhiteSpace(request.OptionText))
            {
                return new UpdateQuizOptionResponse
                {
                    Success = false,
                    Message = "Option text is required."
                };
            }

            option.QuestionId = request.QuestionId;
            option.OptionText = request.OptionText;
            option.IsCorrect = request.IsCorrect;

            await _quizOptionRepository.UpdateQuizOptionAsync(
                option,
                cancellationToken);

            return new UpdateQuizOptionResponse
            {
                Success = true,
                Message = "Quiz option updated successfully.",
                OptionId = option.OptionId
            };
        }
    }
}