using MediatR;
using Microsoft.AspNetCore.Identity;
using RestaurantTraining.Application.Common.Authentication;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public LoginCommandHandler(
            IAuthRepository authRepository,
            IJwtTokenService jwtTokenService)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            // Find user
            var user = await _authRepository.GetUserByEmailAsync(
                request.Email,
                cancellationToken);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            // Check password
            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            // Block inactive accounts
            if (!user.IsActive)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "This account is inactive. Please contact your administrator."
                };
            }

            // Get role (loaded with the user via the RoleId foreign key)
            if (user.Role == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "User role not found."
                };
            }

            var role = user.Role.RoleName;

            // Generate JWT
            var token = _jwtTokenService.GenerateToken(
                user.UserId,
                user.Email,
                user.FullName,
                role);

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                FullName = user.FullName,
                Role = role,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}