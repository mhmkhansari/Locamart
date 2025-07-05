using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Store;

public record AddStoreCommand : ICommand<UnitResult<Error>>
{
    public string Name { get; init; }

    public Guid CategoryId { get; init; }

    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    public string? Bio { get; init; }

    public string? ProfileImage { get; init; }
}

