using BenchmarkDotNet.Attributes;
using Mappy.Converters;
using System.Collections.Generic;
using System.Linq;

namespace Mappy.Benchmark
{
    public class ComplexMap
    {
        public const int Iterations = 50000;
        private Mappy _mappy;
        private Mappy _mappyBaseConverterOnly;
        public IEnumerable<IDictionary<string, object>> TestData { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _mappy = new Mappy();
            _mappyBaseConverterOnly = new Mappy(
                new MappyOptions(
                    converters: new List<ITypeConverter>
                    {
                        new BaseConverter()
                    }));


            _mappy.Map<Customer>(
                GenerateData(1).First());

            _mappyBaseConverterOnly.Map<Customer>(
                GenerateData(1).First());

            Slapper.AutoMapper.Map<Customer>(
                GenerateData(1).First());

            TestData = GenerateData(Iterations)
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
                        LastName = (string)first["LastName"],
                        Orders = x
                            .GroupBy(o => o["Orders_OrderId"])
                            .Select(o =>
                            {
                                var firstOrder = o.First();
                                return new Order
                                {
                                    OrderId = (int)firstOrder["Orders_OrderId"],
                                    OrderTotal = (decimal)firstOrder["Orders_OrderTotal"],
                                    OrderDetails = o
                                        .GroupBy(d => d["Orders_OrderDetails_OrderDetailId"])
                                        .Select(d =>
                                        {
                                            var firstDetail = d.First();
                                            return new OrderDetail
                                            {
                                                OrderDetailId = (int)firstDetail["Orders_OrderDetails_OrderDetailId"],
                                                OrderDetailTotal = (decimal)firstDetail["Orders_OrderDetails_OrderDetailTotal"]
                                            };
                                        }).ToList()
                                };
                            }).ToList()
                    };
                })
                .ToList();
        }

        [Benchmark]
        public void MappyBenchmark()
        {
            _mappy.Map<Customer>(TestData)
                .ToList();
        }

        [Benchmark]
        public void MappyBenchmarkBaseConverterOnly()
        {
            _mappyBaseConverterOnly.Map<Customer>(TestData)
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