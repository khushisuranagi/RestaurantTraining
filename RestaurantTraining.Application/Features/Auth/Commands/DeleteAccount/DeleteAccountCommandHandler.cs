using MediatR;
using Microsoft.AspNetCore.Identity;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Auth.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler
        : IRequestHandler<DeleteAccountCommand, DeleteAccountResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public DeleteAccountCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<DeleteAccountResponse> Handle(
            DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetUserByIdAsync(
                request.UserId, cancellationToken);

            if (user == null)
            {
                return Failure("We couldn't find your account.");
            }

            // Confirm identity with the current password (same as change-password).
            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.CurrentPassword);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Failure("Your password is incorrect.");
            }

            // Permanently deletes the user and all their data.
            await _authRepository.DeleteAccountAsync(request.UserId, cancellationToken);

            return new DeleteAccountResponse
            {
                Success = true,
                Message = "Your account has been deleted."
            };
        }

        private static DeleteAccountResponse Failure(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}
