using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Auth.Commands.UpdateEmail
{
    public class UpdateEmailCommandHandler
        : IRequestHandler<UpdateEmailCommand, UpdateEmailResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UpdateEmailCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UpdateEmailResponse> Handle(
            UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            var newEmail = request.NewEmail.Trim().ToLowerInvariant();

            if (!new EmailAddressAttribute().IsValid(newEmail))
            {
                return Failure("Please enter a valid email address.");
            }

            var user = await _authRepository.GetUserByIdAsync(
                request.UserId, cancellationToken);

            if (user == null)
            {
                return Failure("User not found.");
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.CurrentPassword);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Failure("Your current password is incorrect.");
            }

            if (newEmail == user.Email)
            {
                return Failure("This is already your current email address.");
            }

            if (await _authRepository.EmailExistsAsync(newEmail, cancellationToken))
            {
                return Failure("An account already exists for this email address.");
            }

            user.Email = newEmail;
            await _authRepository.UpdateUserAsync(user, cancellationToken);

            return new UpdateEmailResponse
            {
                Success = true,
                Message = "Your email address has been updated successfully."
            };
        }

        private static UpdateEmailResponse Failure(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}