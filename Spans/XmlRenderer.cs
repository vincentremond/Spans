using System.Text;
using Spans.Xml;

namespace Spans;

public static class XmlRenderer
{
    public static string AsString(IXmlNode parseResult)
    {
        return parseResult switch
        {
            XmlTag tag => RenderTag(tag),
            _ => throw new NotSupportedException("Only XmlTag nodes can be rendered to string."),
        };
    }

    private static string RenderTag(XmlTag tag)
    {
        var result = new StringBuilder();
        tag.RenderXml(result, 0);
        return result.ToString();
    }
}
