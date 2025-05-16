using CSharpFunctionalExtensions;
using Locamart.Shared;

namespace Locamart.Application.Contracts.Services;

public interface IImageStorageService
{
    Task<Result<string,Error>> UploadImageAsync(Stream imageStream, string fileName, string contentType);
}