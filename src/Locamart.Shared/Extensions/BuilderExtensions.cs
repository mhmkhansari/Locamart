namespace Locamart.Shared.Extensions;

public static class BuilderExtensions
{
    public static TBuilder MaybeDo<TBuilder, TValue>(
        this TBuilder builder,
        TValue? value,
        Action<TBuilder, TValue> apply) where TBuilder : class
    {
        if (value is not null)
            apply(builder, value);
        return builder;
    }

    public static TBuilder MaybeDo<TBuilder, T1, T2>(
        this TBuilder builder,
        T1? val1,
        T2? val2,
        Func<T1, T2, bool> condition,
        Action<TBuilder, T1, T2> apply)
        where TBuilder : class
    {
        if (val1 is not null && val2 is not null && condition(val1, val2))
        {
            apply(builder, val1, val2);
        }

        return builder;
    }
}
