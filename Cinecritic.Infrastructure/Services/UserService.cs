using System.Security.Claims;
using System.Text;
using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Services.Users;
using Cinecritic.Infrastructure.Data;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Cinecritic.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        
        public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, DisplayName = dto.DisplayName };

            var result = await _userManager.CreateAsync(user, dto.Password);

            int userId;

            if (!result.Succeeded)
            {
                if (!result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    _logger.LogWarning("Failed to register: invalid request");
                    return Result.Fail(result.Errors.Select(e => new Error(e.Description).WithMetadata("Code", e.Code)));
                }

                user = await _userManager.FindByEmailAsync(dto.Email);

                if (user!.EmailConfirmed)
                {
                    _logger.LogWarning("Failed to register: User with email already exist");
                    return Result.Fail(new Error("Email already exist").WithMetadata("Code", "EmailAlreadyExist"));
                }

                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                {
                    _logger.LogWarning("Failed to register: password incorrect");
                    return Result.Fail(result.Errors.Select(e => new Error(e.Description).WithMetadata("Code", e.Code)));
                }

                userId = user.Id;
            }
            else
            {
                string userIdString = await _userManager.GetUserIdAsync(user);
                userId = int.Parse(userIdString);
            }

            var confirmTokenDto = await GenerateCurrentEmailConfirmationToken(user);

            return Result.Ok(new AuthResultDto { UserId = userId, Code = confirmTokenDto.Value.Code });
        }
        
        public async Task<Result<AuthResultDto>> LoginAsync(LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, false);

            if (result.Succeeded)
            {
                return Result.Ok(new AuthResultDto { EmailNotConfirmed = false });
            }

            _logger.LogWarning("Failed to login");
            return Result.Fail(new Error("Login failed").WithMetadata("Code", "LoginFailed"));
        }
        
        public async Task<Result> ConfirmEmailAsync(ConfirmTokenDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Failed to confirm email: User with UserId={userId} not found", dto.UserId);
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }
            dto.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Code));
            var result = await _userManager.ConfirmEmailAsync(user, dto.Code);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to confirm email: confirm token incorrect for User with UserId={UserId}", dto.UserId);
                return Result.Fail(result.Errors.Select(e => new Error(e.Description).WithMetadata("Code", e.Code)));
            }
            await _signInManager.SignInAsync(user, new AuthenticationProperties());
            return Result.Ok();
        }

        private async Task<Result<ConfirmTokenDto>> GenerateCurrentEmailConfirmationToken(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return Result.Ok(new ConfirmTokenDto { Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)), UserId = user.Id });
        }

        public async Task<Result> ChangeDisplayNameAsync(ChangeDisplayNameDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                _logger.LogWarning("Failed to confirm current email: User with UserId={userId} not found", dto.UserId);
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }
            user.DisplayName = dto.DisplayName;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            return Result.Ok();
        }
        
        public async Task<Result<UserDto>> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Failed to get profile: User with UserId={userId} not found", userId);
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }

            return Result.Ok(new UserDto
            {
                Name = user.DisplayName,
                IsCenturion = user.IsCenturion
            });
        }
    }
}