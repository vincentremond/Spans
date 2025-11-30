namespace Spans;

internal interface IParseResult<out T>
{
    bool IsSuccess { get; }
    T Value { get; }
    int NextIndex { get; }
}

public record struct ParseResult<T> : IParseResult<T>
{
    private readonly T? _value;
    private readonly int? _nextIndex;

    private ParseResult(T? Value, int? NextIndex)
    {
        _value = Value;
        _nextIndex = NextIndex;
    }

    public static ParseResult<T> Success(T value, int nextIndex)
    {
        return new ParseResult<T>(value, nextIndex);
    }

    public static ParseResult<T> Failed()
    {
        return new ParseResult<T>(Value: default, default);
    }

    public bool IsSuccess => _nextIndex.HasValue;

    // ReSharper disable once NullableWarningSuppressionIsUsed
    public T Value => _nextIndex.HasValue ? _value! : throw new InvalidOperationException("No value present.");

    public int NextIndex => _nextIndex.HasValue ? _nextIndex.Value : throw new InvalidOperationException("No next index present.");
}
