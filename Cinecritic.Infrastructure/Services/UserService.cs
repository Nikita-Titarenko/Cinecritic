using System.Text;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Users;
using Cinecritic.Domain.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Cinecritic.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                var verificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, dto.Password);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return Result.Fail(new Error("Incorrect password for unconfirmed account").WithMetadata("Code", "InvalidPassword"));
                }
            }
            else
            {
                existingUser = new ApplicationUser
                {
                    Email = dto.Email,
                    Name = dto.DisplayName
                };
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, dto.Password);

                await _userRepository.AddAsync(existingUser);
            }

            var token = GenerateSimpleToken(existingUser.Id);

            return Result.Ok(new AuthResultDto { UserId = existingUser.Id, Code = token });
        }

        public async Task<Result<AuthResultDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return Result.Fail(new Error("Login failed").WithMetadata("Code", "LoginFailed"));
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return Result.Ok(new AuthResultDto {  UserId = user.Id, Code = GenerateSimpleToken(user.Id) });
            }

            _logger.LogWarning("Failed to login");
            return Result.Fail(new Error("Login failed").WithMetadata("Code", "LoginFailed"));
        }

        public async Task<Result> ChangeDisplayNameAsync(ChangeDisplayNameDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }

            await _userRepository.UpdateDisplayNameAsync(dto.UserId, dto.DisplayName);
            return Result.Ok();
        }

        public async Task<Result<string>> GetNameAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }
            
            return Result.Ok(user.Name);
        }

        private string GenerateSimpleToken(Guid userId)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId.ToString()));
        }
    }
}