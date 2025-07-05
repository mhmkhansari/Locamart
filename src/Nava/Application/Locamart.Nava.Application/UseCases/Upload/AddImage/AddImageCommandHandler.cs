using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Services;

namespace Locamart.Nava.Application.UseCases.Upload.AddImage;

public class AddImageCommandHandler(IImageStorageService imageStorageService) : ICommandHandler<AddImageCommand, Result<AddImageResult, Error>>
{
    public async Task<Result<AddImageResult, Error>> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var result = await imageStorageService.UploadImageAsync(request.File, request.FileName, request.ContentType);

        if (result.IsFailure)
            return Error.Create(result.Error.Code, result.Error.Message);

        return new AddImageResult
        {
            Url = result.Value
        };
    }
}

