using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetLearnerProfile
{
    public class GetLearnerProfileQueryHandler
        : IRequestHandler<GetLearnerProfileQuery, LearnerProfileDto?>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IContentCreatorPeopleRepository _peopleRepository;

        public GetLearnerProfileQueryHandler(
            IAuthRepository authRepository,
            IContentCreatorPeopleRepository peopleRepository)
        {
            _authRepository = authRepository;
            _peopleRepository = peopleRepository;
        }

        public async Task<LearnerProfileDto?> Handle(
            GetLearnerProfileQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetUserByIdAsync(
                request.UserId, cancellationToken);

            if (user is null)
                return null;

            var roles = await _peopleRepository.GetRolesAsync(cancellationToken);
            var roleName = roles.FirstOrDefault(r => r.RoleId == user.RoleId)?.RoleName ?? string.Empty;

            return new LearnerProfileDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = roleName,
                IsActive = user.IsActive
            };
        }
    }
}