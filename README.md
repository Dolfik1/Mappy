# Mappy
Slapper compatible object mapper

## Benchmarks:
#### Simple
```
                          Method |      Mean |     Error |    StdDev |
-------------------------------- |----------:|----------:|----------:|
               StraightBenchmark |  18.77 ms | 0.2298 ms | 0.2149 ms |
                GroupByBenchmark |  58.15 ms | 0.8852 ms | 0.8280 ms |
                  MappyBenchmark | 140.45 ms | 2.7020 ms | 3.2166 ms |
 MappyBenchmarkBaseConverterOnly | 134.08 ms | 2.1920 ms | 2.0504 ms |
                SlapperBenchmark | 599.46 ms | 2.1126 ms | 1.9761 ms |
```
#### Complex
```
                          Method |       Mean |     Error |    StdDev |
-------------------------------- |-----------:|----------:|----------:|
                GroupByBenchmark |   242.3 ms |  1.800 ms | 1.5954 ms |
                  MappyBenchmark |   344.2 ms |  1.990 ms | 1.7642 ms |
 MappyBenchmarkBaseConverterOnly |   330.5 ms |  1.073 ms | 0.8962 ms |
                SlapperBenchmark | 2,966.1 ms | 10.112 ms | 8.4439 ms |
```
