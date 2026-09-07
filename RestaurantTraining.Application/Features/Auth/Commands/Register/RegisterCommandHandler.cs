using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public RegisterCommandHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (request.FullName.Trim().Length < 2)
                return Failure("Please enter your full name.");

            if (!new EmailAddressAttribute().IsValid(email))
                return Failure("Please enter a valid email address.");

            if (request.Password.Length < 8 || !request.Password.Any(char.IsUpper) ||
                !request.Password.Any(char.IsLower) || !request.Password.Any(char.IsDigit))
                return Failure("Password must have at least 8 characters, including upper-case, lower-case, and a number.");

            if (request.Password != request.ConfirmPassword)
                return Failure("Passwords do not match.");

            if (await _authRepository.EmailExistsAsync(email, cancellationToken))
                return Failure("An account already exists for this email address.");

            var role = await _authRepository.GetRoleByIdAsync(request.RoleId, cancellationToken);
            if (role == null || role.RoleName is "Content Creator")
                return Failure("Please select a valid staff role.");

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                PhoneNumber = request.PhoneNumber.Trim(),
                RoleId = role.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _authRepository.AddUserAsync(user, cancellationToken);

            return new RegisterResponse
            {
                Success = true,
                Message = "Your account has been created. You can now log in."
            };
        }

        private static RegisterResponse Failure(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}
