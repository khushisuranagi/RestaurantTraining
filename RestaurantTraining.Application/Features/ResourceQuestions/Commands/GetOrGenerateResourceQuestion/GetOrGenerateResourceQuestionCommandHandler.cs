using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ResourceQuestions.Commands.GetOrGenerateResourceQuestion
{
    public class GetOrGenerateResourceQuestionCommandHandler
        : IRequestHandler<GetOrGenerateResourceQuestionCommand, ResourceQuestionResult>
    {
        private readonly IResourceQuestionRepository _repository;
        private readonly IAiQuestionGenerator _generator;

        public GetOrGenerateResourceQuestionCommandHandler(
            IResourceQuestionRepository repository,
            IAiQuestionGenerator generator)
        {
            _repository = repository;
            _generator = generator;
        }

        public async Task<ResourceQuestionResult> Handle(
            GetOrGenerateResourceQuestionCommand request,
            CancellationToken cancellationToken)
        {
            // Has this learner already answered it correctly? (keeps the gate open)
            var answered = await _repository.HasAnsweredCorrectlyAsync(
                request.UserId, request.ResourceId, cancellationToken);

            // 1) Already generated? Return the stored one.
            var existing = await _repository.GetByResourceIdAsync(
                request.ResourceId, cancellationToken);

            if (existing is not null)
                return Ok(existing.QuestionId, existing.QuestionText, existing.Options, answered);

            // 2) Gather the text context for this resource.
            var context = await _repository.GetResourceContextAsync(
                request.ResourceId, cancellationToken);

            if (context is null)
                return new ResourceQuestionResult { NotAvailable = true };

            // 3) Ask the AI generator for one MCQ.
            var mcq = await _generator.GenerateAsync(new QuestionGenerationRequest
            {
                LessonTitle = context.LessonTitle,
                LessonDescription = context.LessonDescription,
                ResourceType = context.ResourceType,
                ResourceUrl = context.ResourceUrl,
                ContentText = context.ContentText,
                FileName = context.FileName
            }, cancellationToken);

            if (mcq is null)
                return new ResourceQuestionResult { NotAvailable = true };

            // 4) Store it so it's reused next time, then return the learner-safe view.
            var saved = await _repository.SaveGeneratedAsync(
                request.ResourceId, mcq, cancellationToken);

            return Ok(saved.QuestionId, saved.QuestionText, saved.Options, answered);
        }

        private static ResourceQuestionResult Ok(
            int questionId, string questionText, List<StoredResourceOption> options,
            bool alreadyAnswered)
        {
            return new ResourceQuestionResult
            {
                Question = new ResourceQuestionDto
                {
                    QuestionId = questionId,
                    QuestionText = questionText,
                    AlreadyAnsweredCorrectly = alreadyAnswered,
                    Options = options
                        .Select(o => new ResourceQuestionOptionDto
                        {
                            OptionId = o.OptionId,
                            OptionText = o.OptionText   // no IsCorrect!
                        })
                        .ToList()
                }
            };
        }
    }
}
