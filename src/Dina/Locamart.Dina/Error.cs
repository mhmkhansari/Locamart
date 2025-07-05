using CSharpFunctionalExtensions;

namespace Locamart.Dina;

public class Error(string code, string message) : ValueObject<Error>
{

    public string Code { get; } = code;

    public string Message { get; } = message;

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
        return [this];
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
