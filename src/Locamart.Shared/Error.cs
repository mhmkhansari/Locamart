using CSharpFunctionalExtensions;

namespace Locamart.Shared;

public class Error : ValueObject<Error>
{

    public string Code { get; }

    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error Create(string code, string message)
    {
        return new Error(code, message);
    }

    public IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }

    public List<Error> AsList()
    {
        return new List<Error>() { this };
    }

    protected override bool EqualsCore(Error other)
    {
        return (Code == other.Code && Message == other.Message);
    }

    protected override int GetHashCodeCore()
    {
        throw new NotImplementedException();
    }
}
