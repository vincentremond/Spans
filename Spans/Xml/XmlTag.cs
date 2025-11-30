using System.Text;

namespace Spans.Xml;

public record XmlTag(
    ReadOnlyMemory<char> NodeName,
    List<XmlAttribute> Attributes,
    List<IXmlNode> ChildNodes
) : IXmlNode
{
    public void RenderXml(StringBuilder result, int indent)
    {
        result.Append(' ', indent * 4);
        result.Append('<');
        result.Append(NodeName);

        foreach (var attribute in Attributes)
        {
            // result.Append($" {attribute.Name}=\"{attribute.Value}\"");
            result.Append(' ').Append(attribute.Name).Append('=').Append('"').Append(attribute.Value).Append('"');
        }

        result.Append(">").AppendLine();

        foreach (var child in ChildNodes)
        {
            child.RenderXml(result, indent + 1);
        }

        result.Append(' ', indent * 4);
        result.Append("</").Append(NodeName).Append('>').AppendLine();
    }
}
