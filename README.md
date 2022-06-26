[![NuGet](https://img.shields.io/nuget/v/Mappy.svg)](https://www.nuget.org/packages/Mappy/)

# Mappy
Slapper compatible object mapper

## Benchmarks:
#### Simple
```
|                          Method |       Mean |     Error |    StdDev |      Gen 0 |     Gen 1 |    Gen 2 | Allocated |
|-------------------------------- |-----------:|----------:|----------:|-----------:|----------:|---------:|----------:|
|               StraightBenchmark |   6.621 ms | 0.1304 ms | 0.1695 ms |   437.5000 |  203.1250 |  70.3125 |      3 MB |
|                GroupByBenchmark |  23.425 ms | 0.6419 ms | 1.8416 ms |  1375.0000 |  687.5000 | 343.7500 |      8 MB |
|                  MappyBenchmark |  65.253 ms | 1.4543 ms | 4.0300 ms |  4666.6667 | 1666.6667 | 777.7778 |     25 MB |
| MappyBenchmarkBaseConverterOnly |  61.600 ms | 1.2309 ms | 1.8043 ms |  4222.2222 | 1666.6667 | 555.5556 |     24 MB |
|                SlapperBenchmark | 235.378 ms | 4.4259 ms | 4.5451 ms | 12000.0000 | 4000.0000 |        - |     78 MB |
```
#### Complex
```
|                          Method |       Mean |    Error |   StdDev |       Gen 0 |      Gen 1 |    Gen 2 | Allocated |
|-------------------------------- |-----------:|---------:|---------:|------------:|-----------:|---------:|----------:|
|                GroupByBenchmark |   122.1 ms |  1.55 ms |  1.38 ms |  10000.0000 |  3500.0000 | 500.0000 |     59 MB |
|                  MappyBenchmark |   170.9 ms |  3.33 ms |  3.96 ms |  19666.6667 |  5333.3333 | 333.3333 |    119 MB |
| MappyBenchmarkBaseConverterOnly |   161.0 ms |  2.37 ms |  2.22 ms |  18750.0000 |  5250.0000 | 500.0000 |    112 MB |
|                SlapperBenchmark | 1,635.9 ms | 24.84 ms | 23.24 ms | 103000.0000 | 26000.0000 |        - |    621 MB |
```
