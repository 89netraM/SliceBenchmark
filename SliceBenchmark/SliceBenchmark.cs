using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[ReturnValueValidator]
[MemoryDiagnoser, DisassemblyDiagnoser]
[SimpleJob(RuntimeMoniker.Net70), SimpleJob(RuntimeMoniker.Net80), SimpleJob(RuntimeMoniker.Net90)]
public class SliceBenchmark
{
    [Params(8, 1024, 1024 * 1024)]
    public int N;

    private Memory<byte> data;

    [GlobalSetup]
    public void Setup()
    {
        data = new byte[N];
        var span = data.Span;
        for (int i = 0; i < N; i++)
        {
            span[i] = (byte)i;
        }
    }

    [Benchmark(Baseline = true)]
    public int Index()
    {
        var span = data.Span;
        var sum = 0;
        for (int i = 0; i < span.Length; i++)
        {
            sum += span[i];
        }
        return sum;
    }

    [Benchmark]
    public int Slice1()
    {
        var sum = 0;
        for (var span = data.Span; span.Length > 0; span = span.Slice(1))
        {
            sum += span[0];
        }
        return sum;
    }

    [Benchmark]
    public int Slice1Length()
    {
        var sum = 0;
        for (var span = data.Span; span.Length > 0; span = span.Slice(1, span.Length - 1))
        {
            sum += span[0];
        }
        return sum;
    }

    [Benchmark]
    public int Range1()
    {
        var sum = 0;
        for (var span = data.Span; span.Length > 0; span = span[1..])
        {
            sum += span[0];
        }
        return sum;
    }
}
