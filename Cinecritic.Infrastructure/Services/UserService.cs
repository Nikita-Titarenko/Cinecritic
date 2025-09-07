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

namespace Vulyk.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger<UserService> _logger;

        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<UserService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }

        ///  <summary>
        ///  Register new user
        ///  </summary>
        ///  <param name="dto">Data for register new user</param>
        ///  <returns>
        ///  <see cref="AuthResultDto"/> with userId and verifiaction code if
        ///  registration success or error information if it fails
        /// </returns>
        public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, DisplayName = dto.DisplayName };

            var result = await _userManager.CreateAsync(user, dto.Password);

            string userId;

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
                    return Result.Fail(result.Errors.Select(e => new Error(e.Description).WithMetadata("Code", e.Code)));
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
                userId = await _userManager.GetUserIdAsync(user);
            }

            var confirmTokenDto = await GenerateCurrentEmailConfirmationToken(user);

            return Result.Ok(new AuthResultDto { UserId = userId, Code = confirmTokenDto.Value.Code });
        }

        ///  <summary>
        ///  Login for already existing user
        ///  </summary>
        ///  <param name="dto">Data for user login</param>
        ///  <returns>
        ///  <see cref="AuthResultDto"/> containing:
        ///  <list type="bullet">
        ///  <item>UserId and email confirmation token if the email is not confirmed</item>
        ///  <item>UserId and reset confirmation token if the password is not exist</item>
        ///  <item>Error information if it fails</item>
        /// </list>
        /// </returns>
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

        ///  <summary>
        ///  Check email verification token
        ///  </summary>
        ///  <param name="dto">The data containing UserId and email confirmation token</param>
        ///  <returns>
        ///  <see cref="Result"/> containing:
        ///  <list type="bullet">
        ///  <item>Success if token is correct</item>
        ///  <item>Error information if user not found or token is incorrect</item>
        /// </list>
        /// </returns>
        public async Task<Result> ConfirmEmailAsync(ConfirmTokenDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
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

        ///  <summary>
        ///  Generate current email confirmation token for register confirmation
        ///  or email changing
        ///  </summary>
        ///  <param name="email">Current user email</param>
        ///  <returns>
        ///  <see cref="ConfirmTokenDto"/> containing:
        ///  <list type="bullet">
        ///  <item>UserId and reset password token if operation successful</item>
        ///  <item>Error information if user not found</item>
        /// </list>
        /// </returns>
        public async Task<Result<ConfirmTokenDto>> GenerateCurrentEmailConfirmationTokenByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Failed to generate current email confirmation token by email: User not found");
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }

            return await GenerateCurrentEmailConfirmationToken(user);
        }

        ///  <summary>
        ///  Generate current email confirmation token for email changing
        ///  </summary>
        ///  <param name="userId">The identifier of the user</param>
        ///  <returns>
        ///  <see cref="ConfirmTokenDto"/> containing:
        ///  <list type="bullet">
        ///  <item>UserId and reset password token if operation successful</item>
        ///  <item>Error information if user not found</item>
        /// </list>
        /// </returns>
        public async Task<Result<ConfirmTokenDto>> GenerateCurrentEmailConfirmationTokenByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Failed to generate current email confirmation token by id: User with UserId={userId} not found", userId);
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }

            return await GenerateCurrentEmailConfirmationToken(user);
        }

        private async Task<Result<ConfirmTokenDto>> GenerateCurrentEmailConfirmationToken(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return Result.Ok(new ConfirmTokenDto { Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)), UserId = user.Id });
        }

        ///  <summary>
        ///  Confirm current email or 
        ///  add pendingNewEmail for email changing if is not null
        ///  </summary>
        ///  <param name="dto">The data containing UserId and email confirmation token</param>
        ///  <returns>
        ///  <see cref="ConfirmTokenDto"/> containing:
        ///  <list type="bullet">
        ///  <item>Ok if operation successful</item>
        ///  <item>Error information if user not found or token incorrect</item>
        /// </list>
        /// </returns>
        public async Task<Result> ConfirmCurrentEmailAsync(ConfirmTokenDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                _logger.LogWarning("Failed to confirm current email: User with UserId={userId} not found", dto.UserId);
                return Result.Fail(new Error("User not found").WithMetadata("Code", "UserNotFound"));
            }
            dto.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Code));
            var result = await _userManager.ConfirmEmailAsync(user, dto.Code);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to confirm current email: Confirm token incorrect for User with UserId={userId}", dto.UserId);
                return Result.Fail(new Error("Token incorrect").WithMetadata("Code", "TokenIncorrect"));
            }

            return Result.Ok();
        }
    }
}