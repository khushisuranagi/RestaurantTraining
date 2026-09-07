using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.QuizQuestions.Commands.UpdateQuizQuestion
{
    public class UpdateQuizQuestionCommandHandler
        : IRequestHandler<UpdateQuizQuestionCommand, UpdateQuizQuestionResponse>
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;

        public UpdateQuizQuestionCommandHandler(
            IQuizQuestionRepository quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<UpdateQuizQuestionResponse> Handle(
            UpdateQuizQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question =
                await _quizQuestionRepository.GetQuizQuestionByIdAsync(
                    request.QuestionId,
                    cancellationToken);

            if (question == null)
            {
                return new UpdateQuizQuestionResponse
                {
                    Success = false,
                    Message = "Quiz question not found."
                };
            }

            if (string.IsNullOrWhiteSpace(request.QuestionText))
            {
                return new UpdateQuizQuestionResponse
                {
                    Success = false,
                    Message = "Question text is required."
                };
            }

            question.ModuleId = request.ModuleId;
            question.QuestionText = request.QuestionText;
            question.QuestionType = request.QuestionType;
            question.ImageUrl = request.ImageUrl;
            question.Explanation = request.Explanation;
            question.Marks = request.Marks;

            await _quizQuestionRepository.UpdateQuizQuestionAsync(
                question,
                cancellationToken);

            return new UpdateQuizQuestionResponse
            {
                Success = true,
                Message = "Quiz question updated successfully.",
                QuestionId = question.QuestionId
            };
        }
    }
}