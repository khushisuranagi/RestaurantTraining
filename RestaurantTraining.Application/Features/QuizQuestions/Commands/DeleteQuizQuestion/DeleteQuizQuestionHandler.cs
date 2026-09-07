using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.QuizQuestions.Commands.DeleteQuizQuestion
{
    public class DeleteQuizQuestionCommandHandler
        : IRequestHandler<DeleteQuizQuestionCommand, BaseResponse>
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;

        public DeleteQuizQuestionCommandHandler(
            IQuizQuestionRepository quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<BaseResponse> Handle(
            DeleteQuizQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question =
                await _quizQuestionRepository.GetQuizQuestionByIdAsync(
                    request.QuestionId,
                    cancellationToken);

            if (question == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Quiz question not found."
                };
            }

            await _quizQuestionRepository.DeleteQuizQuestionAsync(
                question,
                cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Quiz question deleted successfully."
            };
        }
    }
}