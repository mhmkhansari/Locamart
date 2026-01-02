using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.User.AddUserAddress;

public class AddUserAddressCommand : ICommand<UnitResult<Error>>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PostalCode { get; set; }
    public string? AddressText { get; set; }
    public int CityId { get; set; }
    public int ProvinceId { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
}

