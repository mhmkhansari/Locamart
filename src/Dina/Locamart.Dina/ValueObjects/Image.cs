using CSharpFunctionalExtensions;

namespace Locamart.Dina.ValueObjects;
public sealed class Image : ValueObject<Image>
{
    public string Url { get; }
    public string? AltText { get; }
    public int Ordering { get; }
    public Image(string url, int ordering = 0, string? altText = null)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be null or empty.", nameof(url));

        Url = url;
        AltText = altText;
        Ordering = ordering;
    }

    protected override bool EqualsCore(Image other)
    {
        return Url == other.Url &&
               AltText == other.AltText;
    }

    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(Url, AltText);
    }

    public override string ToString() => Url;
}