using CSharpFunctionalExtensions;

namespace Locamart.Shared;
public sealed class Image : ValueObject<Image>
{
    public string Url { get; }
    public string? AltText { get; }
    public int Width { get; }
    public int Height { get; }

    public Image(string url, int width, int height, string? altText = null)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be null or empty.", nameof(url));

        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero.");

        Url = url;
        Width = width;
        Height = height;
        AltText = altText;
    }

    protected override bool EqualsCore(Image other)
    {
        return Url == other.Url &&
               AltText == other.AltText &&
               Width == other.Width &&
               Height == other.Height;
    }

    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(Url, AltText, Width, Height);
    }

    public override string ToString() => Url;
}