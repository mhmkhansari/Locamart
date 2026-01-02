
namespace Locamart.Nava.Adapter.Http.User.RequestModels;

public class AddUserAddressRequestModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PostalCode { get; set; }
    public string? AddressText { get; set; }
    public int CityId { get; set; }
    public int ProvinceId { get; set; }
    public string Name { get; set; }
}

