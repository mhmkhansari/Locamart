using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Application.Contracts.Services;

public interface IImageStorageService
{
    Task<Result<string, Error>> UploadImageAsync(Stream imageStream, string fileName, string contentType);
}