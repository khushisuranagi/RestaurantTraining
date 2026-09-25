using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.LearnerQuiz.Queries.GetQuiz
{
    public class GetQuizQueryHandler
        : IRequestHandler<GetQuizQuery, LearnerQuizDto?>
    {
        private readonly ILearnerQuizRepository _repository;

        public GetQuizQueryHandler(
            ILearnerQuizRepository repository)
        {
            _repository = repository;
        }

        public async Task<LearnerQuizDto?> Handle(
            GetQuizQuery request,
            CancellationToken cancellationToken)
        {
            var isAssigned = await _repository.IsAssignedToLearnerAsync(
                request.RoleName, request.UserId, request.ModuleId, cancellationToken);

            if (!isAssigned)
                return null;

            var questions = await _repository.GetQuizQuestionsAsync(
                request.ModuleId, cancellationToken);

            return new LearnerQuizDto
            {
                ModuleId = request.ModuleId,
                Questions = questions
                    .Select(q => new LearnerQuizQuestionDto
                    {
                        QuestionId = q.QuestionId,
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        Marks = q.Marks,
                        AllowMultipleAnswers = q.AllowMultipleAnswers,
                        Options = q.Options
                            .Select(o => new LearnerQuizOptionDto
                            {
                                OptionId = o.OptionId,
                                OptionText = o.OptionText
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}
