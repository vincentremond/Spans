namespace Spans.Extensions;

public static class SpanExtensions
{
    extension(ReadOnlySpan<char> str)
    {
        public int LookAhead(int startIndex, Func<char, bool> predicate)
        {
            var finalIndex = startIndex;
            while (finalIndex < str.Length && predicate(str[finalIndex]))
            {
                finalIndex++;
            }

            return finalIndex;
        }

        public bool LookAheadFor(int startIndex, char lookFor)
        {
            if (startIndex >= str.Length)
            {
                return false;
            }

            return str[startIndex] == lookFor;
        }

        public bool LookAheadFor(int startIndex, string lookFor)
        {
            if (startIndex + lookFor.Length > str.Length)
            {
                return false;
            }

            for (var i = 0; i < lookFor.Length; i++)
            {
                if (str[startIndex + i] != lookFor[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
