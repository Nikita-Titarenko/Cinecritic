using System.Text;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Users;
using Cinecritic.Domain.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Cinecritic.Infrastructure.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher<ApplicationUser> passwordHasher,
    ILogger<UserService> logger) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto registrationDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(registrationDto.Email);

        if (existingUser != null)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, registrationDto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Result.Fail(new Error("Incorrect password for unconfirmed account").WithMetadata("Code", "InvalidPassword"));
            }
        }
        else
        {
            existingUser = new ApplicationUser
            {
                Email = registrationDto.Email,
                Name = registrationDto.DisplayName
            };
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, registrationDto.Password);

            await _userRepository.AddAsync(existingUser);
        }

        var token = GenerateSimpleToken(existingUser.Id.ToString());

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
            return Result.Ok(new AuthResultDto { UserId = user.Id, Code = GenerateSimpleToken(user.Id.ToString()) });
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

    public async Task<Result<string>> GetNameAsync(ObjectId userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user == null ? (Result<string>)Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound")) : Result.Ok(user.Name);
    }

    private static string GenerateSimpleToken(string userId)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
    }
}