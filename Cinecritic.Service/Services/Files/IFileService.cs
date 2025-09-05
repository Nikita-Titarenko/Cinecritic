using FluentResults;

namespace Cinecritic.Application.Services.Files
{
    public interface IFileService
    {
        Result<string> GetFilePath(string fileName);
        Task<Result> SaveFile(string fileName, Stream stream);
    }
}