using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using Mappy.Converters;

namespace Mappy.Benchmark
{
    public class ComplexMap
    {
        public const int Iterations = 50000;
        private Mappy Mappy;
        private Mappy MappyBaseConverterOnly;

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
        }

        [Benchmark]
        public void MappyBenchmark()
        {
            Mappy.Map<Customer>(GenerateData(Iterations))
                .ToList();
        }

        [Benchmark]
        public void MappyBenchmarkBaseConverterOnly()
        {
            MappyBaseConverterOnly.Map<Customer>(GenerateData(Iterations))
                .ToList();
        }

        [Benchmark]
        public void SlapperBenchmark()
        {
            // Act
            Slapper.AutoMapper.Map<Customer>(GenerateData(Iterations))
                .ToList();
        }

        private IEnumerable<IDictionary<string, object>> GenerateData(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new Dictionary<string, object>
                {
                    {"CustomerId", i},
                    {"FirstName", "Bob"},
                    {"LastName", "Smith"},
                    {"Orders_OrderId", i},
                    {"Orders_OrderTotal", 50.50m},
                    {"Orders_OrderDetails_OrderDetailId", i},
                    {"Orders_OrderDetails_OrderDetailTotal", 50.50m},
                    {"Orders_OrderDetails_Product_Id", 546},
                    {"Orders_OrderDetails_Product_ProductName", "Black Bookshelf"}
                };
            }
        }
    }
}