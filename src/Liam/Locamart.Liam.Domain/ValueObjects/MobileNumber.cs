using CSharpFunctionalExtensions;
using Locamart.Dina;
using System.Text.RegularExpressions;
using ValueObject = Locamart.Dina.ValueObject;

namespace Locamart.Liam.Domain.ValueObjects;

public class MobileNumber : ValueObject
{
    public string Value { get; private set; }

    public static Result<MobileNumber, Error> Create(string mobileNumber)
    {
        if (!IsValid(mobileNumber))
            Error.Create("invalid_mobile_number", "Invalid mobile number");

        return new MobileNumber(mobileNumber);
    }

    private MobileNumber(string value)
    {
        Value = value;
    }

    private static bool IsValid(string value)
    {
        return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^\+\d{10,15}$");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
