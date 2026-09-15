using MediatR;
using Microsoft.AspNetCore.Identity;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public ChangePasswordCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<ChangePasswordResponse> Handle(
            ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetUserByIdAsync(
                request.UserId, cancellationToken);

            if (user == null)
            {
                return Failure("User not found.");
            }

            var currentPasswordResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.CurrentPassword);

            if (currentPasswordResult == PasswordVerificationResult.Failed)
            {
                return Failure("Your current password is incorrect.");
            }

            if (request.NewPassword.Length < 8 ||
                !request.NewPassword.Any(char.IsUpper) ||
                !request.NewPassword.Any(char.IsLower) ||
                !request.NewPassword.Any(char.IsDigit))
            {
                return Failure("New password must have at least 8 characters, including upper-case, lower-case, and a number.");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return Failure("New passwords do not match.");
            }

            if (currentPasswordResult != PasswordVerificationResult.Failed &&
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.NewPassword)
                    != PasswordVerificationResult.Failed)
            {
                return Failure("New password must be different from your current password.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _authRepository.UpdateUserAsync(user, cancellationToken);

            return new ChangePasswordResponse
            {
                Success = true,
                Message = "Your password has been changed successfully."
            };
        }

        private static ChangePasswordResponse Failure(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}