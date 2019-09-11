using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class NullTests
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

        class DefaultValueTest
        {
            public DefaultValueTest()
            {
                DefaultValue = "Hello, world!";
            }

            public int Id { get; set; }
            public string DefaultValue { get; set; }
        }

        [Fact]
        public void Should_Map_Null_Identifier_Correctly()
        {
            var dictionary = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_OrderId", new int?()},
                {"Orders_OrderTotal", 50.50m},
                {"Orders_OrderDate", DateTime.Now}
            };

            var mappy = new Mappy();
            var customer = mappy.Map<Customer>(dictionary);

            Assert.Equal(dictionary["CustomerId"], customer.CustomerId);
            Assert.Equal(dictionary["FirstName"], customer.FirstName);

            Assert.Single(customer.Orders);

            var order = customer.Orders.Single();
            Assert.Equal(0, order.OrderId);
            Assert.Equal(dictionary["Orders_OrderTotal"], order.OrderTotal);
            Assert.Equal(dictionary["Orders_OrderDate"], order.OrderDate);
        }
    }
}
