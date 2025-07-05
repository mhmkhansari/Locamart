using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.UseCases.Upload.AddImage;

public record AddImageCommand : ICommand<Result<AddImageResult, Error>>
{
    public Stream File { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
}

