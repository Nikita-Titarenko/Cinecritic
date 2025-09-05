using Cinecritic.Application.Services.Files;
using FluentResults;
using Microsoft.AspNetCore.Hosting;

namespace Cinecritic.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private const string UploadPath = "uploads";

        private readonly string _basePath;

        public FileService(IWebHostEnvironment env)
        {
            _basePath = Path.Combine(env.WebRootPath, UploadPath);
        }

        public async Task<Result> SaveFile(string fileName, Stream stream)
        {
            string path = Path.Combine(_basePath, fileName);
            string? directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            using var fileStream = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            return Result.Ok();
        }

        public Result<string> GetFilePath(string fileName)
        {
            string path = Path.Combine(_basePath, fileName);
            if (!File.Exists(path))
            {
                return Result.Fail(new Error("File not exist").WithMetadata("Code", "FileNotExist"));
            }

            return Path.Combine(UploadPath, fileName);
        }
    }
}
