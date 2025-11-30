namespace Spans.Extensions;

public static class ParseResultExtensions
{
    public static T? ToNullable<T>(this ParseResult<T> node)
    {
        return node.IsSuccess ? node.Value : default;
    }
}
