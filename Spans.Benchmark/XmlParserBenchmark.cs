using BenchmarkDotNet.Attributes;

namespace Spans.Benchmark;

[Config(typeof(MyBenchmarkConfig))]
public class XmlParserBenchmark
{
    private readonly string _sampleXmlContent = File.ReadAllText("Sample.xml");

    [Benchmark(Baseline = true)]
    public void Strings()
    {
        StringXmlParser.Parse(_sampleXmlContent);
    }

    [Benchmark]
    public void Spans()
    {
        MemoryXmlParser.Parse(_sampleXmlContent);
    }
}
