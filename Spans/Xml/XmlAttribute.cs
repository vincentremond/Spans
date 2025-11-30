namespace Spans.Xml;

public record XmlAttribute(
    ReadOnlyMemory<char> Name,
    ReadOnlyMemory<char> Value
);
