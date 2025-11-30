using Spans.Extensions;
using Spans.Xml;

namespace Spans;

public static class StringXmlParser
{
    public static IXmlNode Parse(string xmlContent)
    {
        var result = TryParseNode(xmlContent, startIndex: 0);
        if (!result.IsSuccess)
        {
            throw new XmlParsingFailedException("Failed to parse XML content.");
        }

        return result.Value;
    }

    private static IParseResult<IXmlNode> TryParseNode(string xmlContent, int startIndex)
    {
        var tryParseXmlTag = TryParseXmlTag(xmlContent, startIndex);
        if (tryParseXmlTag.IsSuccess)
        {
            return tryParseXmlTag;
        }

        var textNodeResult = TryParseTextNode(xmlContent, startIndex);
        if (textNodeResult.IsSuccess)
        {
            return textNodeResult;
        }

        return ParseResult<IXmlNode>.Failed();
    }

    private static ParseResult<IXmlNode> TryParseTextNode(string xmlContent, int startIndex)
    {
        var currentIndex = xmlContent.LookAhead(startIndex, char.IsWhiteSpace);

        var textStartIndex = currentIndex;
        var afterTextIndex = xmlContent.LookAhead(currentIndex, c => c != '<');

        if (afterTextIndex == textStartIndex)
        {
            return ParseResult<IXmlNode>.Failed();
        }

        var textContent = xmlContent[textStartIndex..afterTextIndex].TrimEnd();
        return ParseResult<IXmlNode>.Success(new XmlTextNode(textContent.AsMemory()), afterTextIndex);
    }

    private static IParseResult<XmlTag> TryParseXmlTag(string xmlContent, int startIndex)
    {
        // Skip leading whitespaces
        var currentIndex = xmlContent.LookAhead(startIndex, char.IsWhiteSpace);
        if (currentIndex >= xmlContent.Length)
        {
            return ParseResult<XmlTag>.Failed();
        }

        if (!xmlContent.LookAheadFor(currentIndex, lookFor: '<'))
        {
            return ParseResult<XmlTag>.Failed();
        }

        currentIndex++; // Skip '<'

        var endOfNodeNameIndex = xmlContent.LookAhead(currentIndex, char.IsLetterOrDigit);

        var nodeNameStart = currentIndex;
        var nodeName = xmlContent[nodeNameStart..endOfNodeNameIndex];

        var attributes = ParseAttributes(xmlContent, endOfNodeNameIndex);
        currentIndex = attributes.NextIndex;

        if (!xmlContent.LookAheadFor(currentIndex, lookFor: '>'))
        {
            return ParseResult<XmlTag>.Failed();
        }

        currentIndex++;

        var childNodes = new List<IXmlNode>();
        while (true)
        {
            var childNodeResult = TryParseNode(xmlContent, currentIndex);
            if (!childNodeResult.IsSuccess)
            {
                break;
            }

            childNodes.Add(childNodeResult.Value);
            // Move currentIndex forward
            currentIndex = childNodeResult.NextIndex;
        }

        // Expect closing tag
        // Skip whitespaces
        currentIndex = xmlContent.LookAhead(currentIndex, char.IsWhiteSpace);
        if (!xmlContent.LookAheadFor(currentIndex, lookFor: '<') ||
            !xmlContent.LookAheadFor(currentIndex + 1, lookFor: '/'))
        {
            return ParseResult<XmlTag>.Failed();
        }

        currentIndex += 2; // Skip '</'

        if (!xmlContent.LookAheadFor(currentIndex, nodeName))
        {
            return ParseResult<XmlTag>.Failed();
        }

        currentIndex += nodeName.Length;

        if (!xmlContent.LookAheadFor(currentIndex, lookFor: '>'))
        {
            return ParseResult<XmlTag>.Failed();
        }

        currentIndex++; // Skip '>'

        return ParseResult<XmlTag>.Success(new XmlTag(nodeName.AsMemory(), attributes.Value, childNodes), currentIndex);
    }

    private static ParseResult<List<XmlAttribute>> ParseAttributes(string xmlContent, int startIndex)
    {
        var attributes = new List<XmlAttribute>();

        var currentIndex = startIndex;

        while (true)
        {
            var attributeResult = TryParseAttribute(xmlContent, currentIndex);
            if (!attributeResult.IsSuccess)
            {
                break;
            }

            attributes.Add(attributeResult.Value);
            currentIndex = attributeResult.NextIndex;
        }

        return ParseResult<List<XmlAttribute>>.Success(attributes, currentIndex);
    }

    private static ParseResult<XmlAttribute> TryParseAttribute(string xmlContent, int currentIndex)
    {
        if (xmlContent.LookAheadFor(currentIndex, lookFor: '>'))
        {
            return ParseResult<XmlAttribute>.Failed();
        }

        //
        var afterSpacesIndex = xmlContent.LookAhead(currentIndex, char.IsWhiteSpace);
        if (afterSpacesIndex == currentIndex)
        {
            return ParseResult<XmlAttribute>.Failed();
        }

        var afterNameIndex = currentIndex = xmlContent.LookAhead(afterSpacesIndex, char.IsLetterOrDigit);

        // Skip '='
        if (!xmlContent.LookAheadFor(afterNameIndex, lookFor: '='))
        {
            return ParseResult<XmlAttribute>.Failed();
        }

        currentIndex++;

        // Skip '"'
        if (!xmlContent.LookAheadFor(currentIndex, lookFor: '"'))
        {
            return ParseResult<XmlAttribute>.Failed();
        }

        currentIndex++;

        var valueStartIndex = currentIndex;
        var afterValueIndex = xmlContent.LookAhead(currentIndex, c => c != '"');
        currentIndex = afterValueIndex + 1;

        var attributeName = xmlContent[afterSpacesIndex..afterNameIndex];
        var attributeValue = xmlContent[valueStartIndex..afterValueIndex];

        return ParseResult<XmlAttribute>.Success(new XmlAttribute(attributeName.AsMemory(), attributeValue.AsMemory()), currentIndex);
    }
}
