# Mappy
Slapper compatible object mapper

## Benchmarks:
#### Simple
```
                          Method |      Mean |     Error |    StdDev |
-------------------------------- |----------:|----------:|----------:|
               StraightBenchmark |  18.71 ms | 0.0532 ms | 0.0416 ms |
                GroupByBenchmark |  58.32 ms | 0.9421 ms | 0.8812 ms |
                  MappyBenchmark | 166.70 ms | 3.4277 ms | 3.2063 ms |
 MappyBenchmarkBaseConverterOnly | 144.54 ms | 2.4099 ms | 2.2543 ms |
                SlapperBenchmark | 594.19 ms | 0.9983 ms | 0.9338 ms |
```
#### Complex
```
                          Method |       Mean |    Error |   StdDev |
-------------------------------- |-----------:|---------:|---------:|
                GroupByBenchmark |   259.7 ms | 1.927 ms | 1.802 ms |
                  MappyBenchmark |   428.5 ms | 2.315 ms | 2.052 ms |
 MappyBenchmarkBaseConverterOnly |   370.9 ms | 6.965 ms | 6.515 ms |
                SlapperBenchmark | 3,000.7 ms | 8.783 ms | 7.786 ms |
```
