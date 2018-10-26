using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Mappy.Benchmark
{
    public class ComplexMap
    {
        public const int Iterations = 50000;
        private IEnumerable<IDictionary<string, object>> Data { get; set; }
        private Mappy Mappy = new Mappy();

        [GlobalSetup]
        public void Setup()
        {
            Data = GenerateData(Iterations);
            Mappy = new Mappy();


            Mappy.Map<Customer>(Data.First());
            Slapper.AutoMapper.Map<Customer>(Data.First());
        }

        [Benchmark]
        public void MappyBenchmark()
        {
            Mappy.Map<Customer>(Data)
                .ToList();
        }

        [Benchmark]
        public void SlapperBenchmark()
        {
            // Act
            Slapper.AutoMapper.Map<Customer>(Data)
                .ToList();
        }

        private IEnumerable<IDictionary<string, object>> GenerateData(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new Dictionary<string, object>
                {
                    { "CustomerId", i },
                    { "FirstName", "Bob" },
                    { "LastName", "Smith" },
                    { "Orders_OrderId", i },
                    { "Orders_OrderTotal", 50.50m },
                    { "Orders_OrderDetails_OrderDetailId", i },
                    { "Orders_OrderDetails_OrderDetailTotal", 50.50m },
                    { "Orders_OrderDetails_Product_Id", 546 },
                    { "Orders_OrderDetails_Product_ProductName", "Black Bookshelf" }
                };
            }
        }
    }
}
