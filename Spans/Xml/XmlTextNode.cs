using System.Text;

namespace Spans.Xml;

public record XmlTextNode(
    ReadOnlyMemory<char> Text
) : IXmlNode
{
    public void RenderXml(StringBuilder result, int indent)
    {
        result.Append(' ', indent * 4).Append(Text).AppendLine();
    }
}
