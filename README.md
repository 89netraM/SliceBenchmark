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

| Method       | Job      | Runtime  | N       | Mean             | Error          | StdDev         | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------- |--------- |--------- |-------- |-----------------:|---------------:|---------------:|------:|--------:|----------:|----------:|------------:|
| **Index**        | **.NET 7.0** | **.NET 7.0** | **8**       |         **3.741 ns** |      **0.0972 ns** |      **0.1229 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | .NET 7.0 | .NET 7.0 | 8       |         4.698 ns |      0.0785 ns |      0.0696 ns |  1.26 |    0.05 |     169 B |         - |          NA |
| Slice1Length | .NET 7.0 | .NET 7.0 | 8       |         5.620 ns |      0.1235 ns |      0.1212 ns |  1.50 |    0.05 |     176 B |         - |          NA |
| Range1       | .NET 7.0 | .NET 7.0 | 8       |        13.806 ns |      0.1067 ns |      0.0945 ns |  3.69 |    0.14 |     244 B |         - |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |
| **Index**        | **.NET 8.0** | **.NET 8.0** | **8**       |         **3.719 ns** |      **0.0634 ns** |      **0.0593 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | .NET 8.0 | .NET 8.0 | 8       |         4.841 ns |      0.0816 ns |      0.0682 ns |  1.30 |    0.03 |     169 B |         - |          NA |
| Slice1Length | .NET 8.0 | .NET 8.0 | 8       |         5.442 ns |      0.0902 ns |      0.0844 ns |  1.46 |    0.04 |     176 B |         - |          NA |
| Range1       | .NET 8.0 | .NET 8.0 | 8       |        13.755 ns |      0.0892 ns |      0.0791 ns |  3.70 |    0.07 |     232 B |         - |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |
| **Index**        | **.NET 9.0** | **.NET 9.0** | **8**       |         **3.585 ns** |      **0.0478 ns** |      **0.0447 ns** |  **1.00** |    **0.00** |     **153 B** |         **-** |          **NA** |
| Slice1       | .NET 9.0 | .NET 9.0 | 8       |         4.837 ns |      0.0588 ns |      0.0521 ns |  1.35 |    0.02 |     168 B |         - |          NA |
| Slice1Length | .NET 9.0 | .NET 9.0 | 8       |         5.439 ns |      0.0935 ns |      0.0829 ns |  1.52 |    0.03 |     176 B |         - |          NA |
| Range1       | .NET 9.0 | .NET 9.0 | 8       |        13.504 ns |      0.1261 ns |      0.0985 ns |  3.76 |    0.04 |     239 B |         - |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |

| Method       | Job      | Runtime  | N       | Mean             | Error          | StdDev         | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------- |--------- |--------- |-------- |-----------------:|---------------:|---------------:|------:|--------:|----------:|----------:|------------:|
| **Index**        | **.NET 7.0** | **.NET 7.0** | **1024**    |       **339.674 ns** |      **6.8184 ns** |      **7.8521 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | .NET 7.0 | .NET 7.0 | 1024    |       413.538 ns |      5.2666 ns |      4.9264 ns |  1.21 |    0.04 |     169 B |         - |          NA |
| Slice1Length | .NET 7.0 | .NET 7.0 | 1024    |       569.044 ns |      8.4315 ns |      9.7097 ns |  1.68 |    0.03 |     176 B |         - |          NA |
| Range1       | .NET 7.0 | .NET 7.0 | 1024    |     1,171.017 ns |     15.5423 ns |     14.5383 ns |  3.44 |    0.11 |     244 B |         - |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |
| **Index**        | **.NET 8.0** | **.NET 8.0** | **1024**    |       **341.011 ns** |      **6.7434 ns** |      **9.0023 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | .NET 8.0 | .NET 8.0 | 1024    |       429.799 ns |      7.7181 ns |      6.8419 ns |  1.26 |    0.04 |     169 B |         - |          NA |
| Slice1Length | .NET 8.0 | .NET 8.0 | 1024    |       581.061 ns |     11.0846 ns |     10.3686 ns |  1.71 |    0.06 |     176 B |         - |          NA |
| Range1       | .NET 8.0 | .NET 8.0 | 1024    |     1,019.309 ns |     12.2096 ns |     11.4209 ns |  3.00 |    0.08 |     232 B |         - |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |
| **Index**        | **.NET 9.0** | **.NET 9.0** | **1024**    |       **355.403 ns** |      **2.7215 ns** |      **2.5457 ns** |  **1.00** |    **0.00** |     **153 B** |         **-** |          **NA** |
| Slice1       | .NET 9.0 | .NET 9.0 | 1024    |       416.234 ns |      8.1586 ns |      8.0128 ns |  1.17 |    0.02 |     168 B |         - |          NA |
| Slice1Length | .NET 9.0 | .NET 9.0 | 1024    |       584.781 ns |      9.7190 ns |      9.0911 ns |  1.65 |    0.03 |     176 B |         - |          NA |
| Range1       | .NET 9.0 | .NET 9.0 | 1024    |     1,175.905 ns |     14.6635 ns |     12.9988 ns |  3.31 |    0.04 |     239 B |         - |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |

| Method       | Job      | Runtime  | N       | Mean             | Error          | StdDev         | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------- |--------- |--------- |-------- |-----------------:|---------------:|---------------:|------:|--------:|----------:|----------:|------------:|
| **Index**        | **.NET 7.0** | **.NET 7.0** | **1048576** |   **364,102.017 ns** |  **7,187.8235 ns** |  **8,277.5082 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | .NET 7.0 | .NET 7.0 | 1048576 |   421,267.844 ns |  8,063.6678 ns | 10,764.7649 ns |  1.16 |    0.05 |     169 B |         - |          NA |
| Slice1Length | .NET 7.0 | .NET 7.0 | 1048576 |   606,240.163 ns |  6,466.7242 ns |  6,048.9779 ns |  1.67 |    0.04 |     176 B |         - |          NA |
| Range1       | .NET 7.0 | .NET 7.0 | 1048576 | 1,221,655.247 ns | 17,170.4710 ns | 16,061.2692 ns |  3.36 |    0.10 |     244 B |       1 B |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |
| **Index**        | **.NET 8.0** | **.NET 8.0** | **1048576** |   **364,476.685 ns** |  **2,000.5158 ns** |  **1,773.4052 ns** |  **1.00** |    **0.00** |     **156 B** |         **-** |          **NA** |
| Slice1       | .NET 8.0 | .NET 8.0 | 1048576 |   430,485.781 ns |  5,676.4136 ns |  5,031.9928 ns |  1.18 |    0.01 |     169 B |         - |          NA |
| Slice1Length | .NET 8.0 | .NET 8.0 | 1048576 |   612,366.882 ns | 10,646.1680 ns | 10,455.9605 ns |  1.68 |    0.03 |     176 B |         - |          NA |
| Range1       | .NET 8.0 | .NET 8.0 | 1048576 | 1,004,374.972 ns | 11,642.7641 ns | 10,321.0071 ns |  2.76 |    0.03 |     232 B |       1 B |          NA |
|              |          |          |         |                  |                |                |       |         |           |           |             |
| **Index**        | **.NET 9.0** | **.NET 9.0** | **1048576** |   **356,519.102 ns** |  **6,812.2983 ns** |  **6,372.2281 ns** |  **1.00** |    **0.00** |     **153 B** |         **-** |          **NA** |
| Slice1       | .NET 9.0 | .NET 9.0 | 1048576 |   420,045.492 ns |  8,152.7336 ns | 11,428.9939 ns |  1.18 |    0.04 |     168 B |         - |          NA |
| Slice1Length | .NET 9.0 | .NET 9.0 | 1048576 |   577,878.696 ns |  8,547.2397 ns |  9,843.0139 ns |  1.62 |    0.04 |     176 B |         - |          NA |
| Range1       | .NET 9.0 | .NET 9.0 | 1048576 |   885,008.859 ns | 17,475.1949 ns | 15,491.3051 ns |  2.49 |    0.06 |     239 B |         - |          NA |

Also, the assembly produced as shown by BenchmarkDotNet (and SharpLab) is
different between the `Slice1Length` and `Range1` methods.
