using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Profile.Queries.GetProfile
{
    public class GetProfileQueryHandler
        : IRequestHandler<GetProfileQuery, ProfileDto?>
    {
        private readonly IProfileRepository _repository;

        public GetProfileQueryHandler(
            IProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProfileDto?> Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _repository.GetProfileAsync(
                request.UserId, cancellationToken);

            if (profile is null)
                return null;

            return new ProfileDto
            {
                UserId = profile.UserId,
                FullName = profile.FullName,
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber,
                Role = profile.Role,
                IsActive = profile.IsActive,
                CreatedAt = profile.CreatedAt
            };
        }
    }
}
