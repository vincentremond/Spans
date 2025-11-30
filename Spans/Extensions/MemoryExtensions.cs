namespace Spans.Extensions;

public static class MemoryExtensions
{
    extension(ReadOnlyMemory<char> str)
    {
        public int LookAhead(int startIndex, Func<char, bool> predicate)
        {
            var finalIndex = startIndex;
            while (finalIndex < str.Length && predicate(str.Span[finalIndex]))
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

            return str.Span[startIndex] == lookFor;
        }

        public bool LookAheadFor(int startIndex, ReadOnlyMemory<char> lookFor)
        {
            if (startIndex + lookFor.Length > str.Length)
            {
                return false;
            }

            for (var i = 0; i < lookFor.Length; i++)
            {
                if (str.Span[startIndex + i] != lookFor.Span[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
