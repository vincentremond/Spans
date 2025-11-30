using AwesomeAssertions;

namespace Spans.Tests;

public class RoundTripParseAndSerializeTests
{
    [Test]
    public void Strings()
    {
        // Arrange
        var sampleContent = File.ReadAllText("Sample.xml");

        // Act
        var parseResult = StringXmlParser.Parse(sampleContent);

        // Assert 
        var roundTripXml = XmlRenderer.AsString(parseResult);
        roundTripXml.Should().Be(sampleContent);
    }
    [Test]
    public void Memory()
    {
        // Arrange
        var sampleContent = File.ReadAllText("Sample.xml");

        // Act
        var parseResult = MemoryXmlParser.Parse(sampleContent);

        // Assert 
        var roundTripXml = XmlRenderer.AsString(parseResult);
        roundTripXml.Should().Be(sampleContent);
    }
}