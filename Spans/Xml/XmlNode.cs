using System.Text;

namespace Spans.Xml;

public interface IXmlNode
{
    void RenderXml(StringBuilder result, int indent);
}