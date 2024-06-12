# SliceBenchmark

Benchmark for looping through a `Span<T>` with either indexing, slicing off the
first element with the `Slice` method (without and with explicitly specifying
the new length), or slicing of the first element with a range (`1..`).

[SharpLab shows](https://sharplab.io/#v2:D4AQTAjAsAULIGYAE4kGUA2BLAxgUwCE8A7HACwFsBDAJwGtYBvWJVpABxqwDcqAXPEgCyeCgHsaATwA8AI0kCAfEgAm/KgG5YLNoiRZifJAEliKvAA8AFAEodrZjDbOkvGkgDO7KsSQBeVXUAOjRvYi0nF1Y3TwBXCn8kAAYIqNYAMwkkKwMjLESU/SRpTzCggBkSAHM+Mg19AGoGu0ioxzSXD3ikBoCvHwBtLABdVLSAX3sXEAB2OIox1km4VpRkXPRsfAhbKfa0mK6EgJSp50z3K0OwxLU+KhCw+v7iCurapGVCl8SXkK28FYIDYWh19h1WEcen0wgMkqMzmxlh1ZvNFkhllM9BtMLg8BBKsQamRdqtwS5Dt0TujzlkrrRSj5bsFQj5nmVCcTPsl2UyYT5/nigQAaRmvTkfAC0SGBoLS5I6UN6YrhCNWzmRaVRR3RmNW2MMSAASj4qvjSc4FWxKcceYiMnTrnzAvdHmyxW8iR8vrzfPziAMIEEgsM5W17Z1usqXqqaUiI9r4rrYMsgA==)
the range version will be lowered to using the `Slice` method with explicit new
length.

```csharp
span = span[1..];
```
is lowered to
```csharp
span = span.Slice(1, span.Length - 1);
```

But judging by the benchmarks that doesn't seem to reflect reality.

| Method       | N       | Mean             | Error          | StdDev         | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------- |--------:|-----------------:|---------------:|---------------:|------:|--------:|----------:|----------:|------------:|
| **Index**        | **8**       |         **3.811 ns** |      **0.0963 ns** |      **0.0946 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | 8       |         4.872 ns |      0.0386 ns |      0.0322 ns |  1.28 |    0.03 |     169 B |         - |          NA |
| Slice1Length | 8       |         5.566 ns |      0.0759 ns |      0.0710 ns |  1.46 |    0.04 |     176 B |         - |          NA |
| Range1       | 8       |        13.704 ns |      0.1059 ns |      0.0938 ns |  3.59 |    0.09 |     232 B |         - |          NA |
|              |         |                  |                |                |       |         |           |           |             |
| **Index**        | **1024**    |       **337.335 ns** |      **5.4063 ns** |      **5.0571 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | 1024    |       415.330 ns |      5.2002 ns |      4.6098 ns |  1.23 |    0.03 |     169 B |         - |          NA |
| Slice1Length | 1024    |       564.208 ns |      3.7797 ns |      3.1562 ns |  1.68 |    0.03 |     176 B |         - |          NA |
| Range1       | 1024    |     1,003.689 ns |     19.2070 ns |     17.9662 ns |  2.98 |    0.07 |     232 B |         - |          NA |
|              |         |                  |                |                |       |         |           |           |             |
| **Index**        | **1048576** |   **333,869.409 ns** |  **3,098.6093 ns** |  **2,746.8365 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | 1048576 |   402,109.905 ns |  2,382.7502 ns |  1,989.7043 ns |  1.20 |    0.01 |     169 B |         - |          NA |
| Slice1Length | 1048576 |   575,577.536 ns | 11,057.9516 ns | 10,343.6148 ns |  1.73 |    0.04 |     176 B |         - |          NA |
| Range1       | 1048576 | 1,033,553.138 ns | 19,597.4235 ns | 18,331.4421 ns |  3.09 |    0.05 |     232 B |       1 B |          NA |

Also, the assembly produced as shown by BenchmarkDotNet (and SharpLab) is
different between the `Slice1Length` and `Range1` methods.
