using CSharpFunctionalExtensions;
using Locamart.Dina;
using System.Text.RegularExpressions;

namespace Locamart.Liam.Domain.ValueObjects;

public record MobileNumber
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
}
