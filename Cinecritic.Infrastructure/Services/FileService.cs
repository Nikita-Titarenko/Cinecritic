using Cinecritic.Application.Services.Files;
using FluentResults;
using Microsoft.AspNetCore.Hosting;

namespace Cinecritic.Infrastructure.Services;

public class FileService(IWebHostEnvironment env) : IFileService
{
    private const string UploadPath = "uploads";

    private readonly string _basePath = Path.Combine(env.WebRootPath, UploadPath);

    public async Task<Result> SaveFile(string fileName, Stream stream)
    {
        string path = Path.Combine(_basePath, fileName);
        string? directoryName = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        await using var fileStream = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fileStream);
        return Result.Ok();
    }

    public Result<string> GetFilePath(string fileName)
    {
        string path = Path.Combine(_basePath, fileName);
        return !File.Exists(path)
            ? (Result<string>)Result.Fail(new Error("File not exist").WithMetadata("Code", "FileNotExist"))
            : (Result<string>)Path.Combine(UploadPath, fileName);
    }
}
