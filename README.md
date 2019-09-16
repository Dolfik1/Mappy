[![NuGet](https://img.shields.io/nuget/v/Mappy.svg)](https://www.nuget.org/packages/Mappy/)

# Mappy
Slapper compatible object mapper

## Benchmarks:
#### Simple
```
|                          Method |       Mean |     Error |    StdDev |
|-------------------------------- |-----------:|----------:|----------:|
|               StraightBenchmark |   7.742 ms | 0.1541 ms | 0.1893 ms |
|                GroupByBenchmark |  25.911 ms | 0.3059 ms | 0.2554 ms |
|                  MappyBenchmark |  65.875 ms | 1.7070 ms | 1.7530 ms |
| MappyBenchmarkBaseConverterOnly |  62.070 ms | 1.1959 ms | 1.5964 ms |
|                SlapperBenchmark | 699.924 ms | 2.3368 ms | 1.8244 ms |
```
#### Complex
```
|                          Method |       Mean |     Error |    StdDev |
|-------------------------------- |-----------:|----------:|----------:|
|                GroupByBenchmark |   150.6 ms |  2.943 ms |  5.232 ms |
|                  MappyBenchmark |   187.1 ms |  2.109 ms |  1.646 ms |
| MappyBenchmarkBaseConverterOnly |   181.4 ms |  3.414 ms |  2.851 ms |
|                SlapperBenchmark | 3,729.6 ms | 70.293 ms | 72.185 ms |
```
