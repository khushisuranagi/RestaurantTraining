using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ResourceQuestions.Queries.GetModulePoints
{
    public class GetModulePointsQueryHandler
        : IRequestHandler<GetModulePointsQuery, int>
    {
        private readonly IResourceQuestionRepository _repository;

        public GetModulePointsQueryHandler(IResourceQuestionRepository repository)
        {
            _repository = repository;
        }

        public Task<int> Handle(
            GetModulePointsQuery request, CancellationToken cancellationToken)
            => _repository.GetModulePointsAsync(
                request.UserId, request.ModuleId, cancellationToken);
    }
}
