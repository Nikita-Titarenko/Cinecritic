using Cinecritic.Application.DTOs.Account;
using FluentResults;
using MongoDB.Bson;

namespace Cinecritic.Application.Services.Users
{
    public interface IUserService
    {
        Task<Result<AuthResultDto>> RegisterAsync(RegisterDto registrationDto);
        Task<Result<AuthResultDto>> LoginAsync(LoginDto dto);
        Task<Result> ChangeDisplayNameAsync(ChangeDisplayNameDto dto);
        Task<Result<string>> GetNameAsync(ObjectId userId);
    }
}