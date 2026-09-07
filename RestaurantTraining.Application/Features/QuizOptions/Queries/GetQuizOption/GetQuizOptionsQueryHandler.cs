using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
//using RestaurantTraining.Application.Features.QuizOptions.Queries.GetQuizOption;

namespace RestaurantTraining.Application.Features.QuizOptions.Queries.GetQuizOptions
{
    public class GetQuizOptionsQueryHandler
        : IRequestHandler<GetQuizOptionsQuery, List<QuizOptionDto>>
    {
        private readonly IQuizOptionRepository _quizOptionRepository;

        public GetQuizOptionsQueryHandler(
            IQuizOptionRepository quizOptionRepository)
        {
            _quizOptionRepository = quizOptionRepository;
        }

        public async Task<List<QuizOptionDto>> Handle(
            GetQuizOptionsQuery request,
            CancellationToken cancellationToken)
        {
            var options =
                await _quizOptionRepository.GetAllQuizOptionsAsync(
                    cancellationToken);

            return options.Select(x => new QuizOptionDto
            {
                OptionId = x.OptionId,
                QuestionId = x.QuestionId,
                OptionText = x.OptionText,
                IsCorrect = x.IsCorrect
            }).ToList();
        }
    }
}