using Cinecritic.Application.DTOs.Account;
using FluentResults;

namespace Cinecritic.Application.Services.Users
{
    public interface IUserService
    {
        Task<Result<AuthResultDto>> RegisterAsync(RegisterDto registrationDto);
        Task<Result<AuthResultDto>> LoginAsync(LoginDto dto);
        Task<Result> ChangeDisplayNameAsync(ChangeDisplayNameDto dto);
        Task<Result<string>> GetNameAsync(Guid userId);
    }
}