using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class OptimizationTests
    {
        class Customer
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public ICollection<Order> Orders { get; set; }
        }

        class Order
        {
            public int OrderId { get; set; }
            public decimal OrderTotal { get; set; }
            public DateTime? OrderDate { get; set; }
        }

        [Fact]
        public void Should_Map_Different_Sets_Correctly()
        {
            // Arrange

            var partialInfo = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_OrderId", 1}
            };


            var fullInfo = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_OrderId", 1},
                {"Orders_OrderTotal", 50.50m},
                {"Orders_OrderDate", DateTime.Now}
            };


            var mappy = new Mappy();

            // Act
            var customerPartial = mappy.Map<Customer>(partialInfo);
            var customerFull = mappy.Map<Customer>(fullInfo);

            Assert.NotNull(customerPartial);
            Assert.Equal(partialInfo["CustomerId"], customerPartial.CustomerId);
            Assert.Equal(partialInfo["FirstName"], customerPartial.FirstName);
            Assert.Equal(partialInfo["LastName"], customerPartial.LastName);
            Assert.Single(customerPartial.Orders);

            var order = customerPartial.Orders.Single();
            Assert.Equal(partialInfo["Orders_OrderId"], order.OrderId);
            Assert.Equal(default(decimal), order.OrderTotal);
            Assert.Null(order.OrderDate);


            Assert.Equal(fullInfo["CustomerId"], customerFull.CustomerId);
            Assert.Equal(fullInfo["FirstName"], customerFull.FirstName);
            Assert.Equal(fullInfo["LastName"], customerFull.LastName);
            Assert.Single(customerFull.Orders);

            order = customerFull.Orders.Single();

            Assert.Equal(fullInfo["Orders_OrderId"], order.OrderId);
            Assert.Equal(fullInfo["Orders_OrderTotal"], order.OrderTotal);
            Assert.Equal(fullInfo["Orders_OrderDate"], order.OrderDate);



        }
    }
}
