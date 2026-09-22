using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Commands.SetLearnerActiveStatus
{
    public class SetLearnerActiveStatusCommandHandler
        : IRequestHandler<SetLearnerActiveStatusCommand, SetLearnerActiveStatusResult>
    {
        private readonly IAuthRepository _authRepository;

        public SetLearnerActiveStatusCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<SetLearnerActiveStatusResult> Handle(
            SetLearnerActiveStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetUserByIdAsync(
                request.UserId, cancellationToken);

            if (user is null)
            {
                return new SetLearnerActiveStatusResult
                {
                    Success = false,
                    Message = "User not found.",
                    IsActive = false
                };
            }

            user.IsActive = request.IsActive;

            await _authRepository.UpdateUserAsync(user, cancellationToken);

            return new SetLearnerActiveStatusResult
            {
                Success = true,
                Message = request.IsActive ? "Account set to active." : "Account set to inactive.",
                IsActive = user.IsActive
            };
        }
    }
}