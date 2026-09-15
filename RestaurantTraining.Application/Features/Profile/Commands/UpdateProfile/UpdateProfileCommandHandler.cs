using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Profile.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler
        : IRequestHandler<UpdateProfileCommand, UpdateProfileResult>
    {
        private readonly IProfileRepository _repository;

        public UpdateProfileCommandHandler(
            IProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateProfileResult> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            var fullName = request.FullName?.Trim() ?? string.Empty;
            var phoneNumber = request.PhoneNumber?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                return new UpdateProfileResult
                {
                    Success = false,
                    Message = "Full name is required."
                };
            }

            var updated = await _repository.UpdateProfileAsync(
                request.UserId, fullName, phoneNumber, cancellationToken);

            if (!updated)
            {
                return new UpdateProfileResult
                {
                    Success = false,
                    Message = "We couldn't find your account."
                };
            }

            return new UpdateProfileResult
            {
                Success = true,
                Message = "Profile updated."
            };
        }
    }
}
