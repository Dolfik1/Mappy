using BenchmarkDotNet.Attributes;
using Mappy.Converters;
using System.Collections.Generic;
using System.Linq;

namespace Mappy.Benchmark
{
    public class SimpleMap
    {
        public const int Iterations = 50000;
        private Mappy Mappy;
        private Mappy MappyBaseConverterOnly;
        private IEnumerable<IDictionary<string, object>> TestData { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Mappy = new Mappy();
            MappyBaseConverterOnly = new Mappy(
                new MappyOptions(
                    converters: new List<ITypeConverter>
                    {
                        new BaseConverter()
                    }));



            Mappy.Map<Customer>(
                GenerateData(1).First());

            MappyBaseConverterOnly.Map<Customer>(
                GenerateData(1).First());

            Slapper.AutoMapper.Map<Customer>(
                GenerateData(1).First());

            TestData = GenerateData(Iterations).ToList();
        }

        [Benchmark]
        public void StraightBenchmark()
        {
            TestData
                .Select(x => new Customer
                {
                    CustomerId = (int)x["CustomerId"],
                    FirstName = (string)x["FirstName"],
                    LastName = (string)x["LastName"]
                })
                .ToList();
        }

        [Benchmark]
        public void GroupByBenchmark()
        {
            TestData
                .GroupBy(x => x["CustomerId"])
                .Select(x =>
                {
                    var first = x.First();
                    return new Customer
                    {
                        CustomerId = (int)first["CustomerId"],
                        FirstName = (string)first["FirstName"],
                        LastName = (string)first["LastName"]
                    };
                })
                .ToList();
        }

        [Benchmark]
        public void MappyBenchmark()
        {
            Mappy.Map<Customer>(TestData)
                .ToList();
        }

        [Benchmark]
        public void MappyBenchmarkBaseConverterOnly()
        {
            MappyBaseConverterOnly.Map<Customer>(TestData)
                .ToList();
        }

#if SLAPPER_BENCHMARK
        [Benchmark]
        public void SlapperBenchmark()
        {
            // Act
            Slapper.AutoMapper.Map<Customer>(TestData)
                .ToList();
        }
#endif
        private IEnumerable<IDictionary<string, object>> GenerateData(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new Dictionary<string, object>
                {
                    { "CustomerId", i },
                    { "FirstName", "Bob" },
                    { "LastName", "Smith" }
                };
            }
        }
    }
}
