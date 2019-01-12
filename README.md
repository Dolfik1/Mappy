[![NuGet](https://img.shields.io/nuget/v/Mappy.svg)](https://www.nuget.org/packages/Mappy/)

# Mappy
Slapper compatible object mapper

## Benchmarks:
#### Simple
```
                          Method |      Mean |     Error |    StdDev |
-------------------------------- |----------:|----------:|----------:|
               StraightBenchmark |  18.44 ms | 0.2827 ms | 0.2644 ms |
                GroupByBenchmark |  57.45 ms | 0.8098 ms | 0.7575 ms |
                  MappyBenchmark | 123.45 ms | 1.9327 ms | 1.8079 ms |
 MappyBenchmarkBaseConverterOnly | 123.51 ms | 2.3070 ms | 2.3691 ms |
                SlapperBenchmark | 596.42 ms | 1.1709 ms | 1.0953 ms |
```
#### Complex
```
                          Method |       Mean |     Error |    StdDev |
-------------------------------- |-----------:|----------:|----------:|
                GroupByBenchmark |   239.6 ms |  1.156 ms |  1.081 ms |
                  MappyBenchmark |   337.2 ms |  6.845 ms |  6.402 ms |
 MappyBenchmarkBaseConverterOnly |   326.1 ms |  6.216 ms |  7.400 ms |
                SlapperBenchmark | 3,031.4 ms | 37.129 ms | 34.730 ms |
```
