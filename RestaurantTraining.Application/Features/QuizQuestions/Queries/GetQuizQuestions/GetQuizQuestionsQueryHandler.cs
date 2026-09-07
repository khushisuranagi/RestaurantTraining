using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.QuizQuestions.Queries.GetQuizQuestions
{
    public class GetQuizQuestionsQueryHandler
        : IRequestHandler<GetQuizQuestionsQuery, List<QuizQuestionDto>>
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;

        public GetQuizQuestionsQueryHandler(
            IQuizQuestionRepository quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<List<QuizQuestionDto>> Handle(
            GetQuizQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var questions =
                await _quizQuestionRepository.GetAllQuizQuestionsAsync(
                    cancellationToken);

            return questions.Select(x => new QuizQuestionDto
            {
                QuestionId = x.QuestionId,
                ModuleId = x.ModuleId,
                QuestionText = x.QuestionText,
                QuestionType = x.QuestionType,
                ImageUrl = x.ImageUrl,
                Explanation = x.Explanation,
                Marks = x.Marks
            }).ToList();
        }
    }
}