using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class GenericTests
    {
        class Customer
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public ICollection<Order<int>> Orders { get; set; }
        }

        class Order<T>
        {
            public T OrderId { get; set; }
            public decimal OrderTotal { get; set; }
        }

        [Fact]
        public void Can_Map_And_Ignore_Keys_Case()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"CUSTOMERID", 1},
                {"FIRSTNAME", "Bob"},
                {"LASTNAME", "Smith"},
                {"ORDERS_ORDERID", 1},
                {"ORDERS_ORDERTOTAL", 50.50m}
            };


            var options = new MappyOptions(
                stringComparison: StringComparison.OrdinalIgnoreCase,
                useDefaultDictionaryComparer: false);


            var mappy = new Mappy(options);

            // Act
            var customer = mappy.Map<Customer>(dictionary);

            // Assert

            Assert.Equal(dictionary["CUSTOMERID"], customer.CustomerId);
            Assert.Equal(dictionary["FIRSTNAME"], customer.FirstName);
            Assert.Equal(dictionary["LASTNAME"], customer.LastName);

            Assert.Single(customer.Orders);
            Assert.Equal(dictionary["ORDERS_ORDERID"], customer.Orders.Single().OrderId);
            Assert.Equal(dictionary["ORDERS_ORDERTOTAL"], customer.Orders.Single().OrderTotal);
        }

        [Fact]
        public void Can_Map_And_Ignore_Keys_Case_Multiple()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"CUSTOMERID", 1},
                {"FIRSTNAME", "Bob"},
                {"LASTNAME", "Smith"},
                {"ORDERS_ORDERID", 1},
                {"ORDERS_ORDERTOTAL", 50.50m}
            };

            // Arrange
            var dictionary2 = new Dictionary<string, object>
            {
                {"CUSTOMERID", 1},
                {"FIRSTNAME", "Bob"},
                {"LASTNAME", "Smith"},
                {"ORDERS_ORDERID", 2},
                {"ORDERS_ORDERTOTAL", 30.30m}
            };


            var options = new MappyOptions(
                stringComparison: StringComparison.OrdinalIgnoreCase,
                useDefaultDictionaryComparer: false);


            var mappy = new Mappy(options);

            // Act
            var customers = mappy.Map<Customer>(new List<Dictionary<string, object>>
            {
                dictionary,
                dictionary2
            });

            // Assert

            Assert.Single(customers);

            var customer = customers.Single();

            Assert.Equal(dictionary["CUSTOMERID"], customer.CustomerId);
            Assert.Equal(dictionary["FIRSTNAME"], customer.FirstName);
            Assert.Equal(dictionary["LASTNAME"], customer.LastName);

            Assert.Equal(2, customer.Orders.Count);
            Assert.Equal(dictionary["ORDERS_ORDERID"], customer.Orders.First().OrderId);
            Assert.Equal(dictionary["ORDERS_ORDERTOTAL"], customer.Orders.First().OrderTotal);

            Assert.Equal(dictionary2["ORDERS_ORDERID"], customer.Orders.Last().OrderId);
            Assert.Equal(dictionary2["ORDERS_ORDERTOTAL"], customer.Orders.Last().OrderTotal);
        }
    }
}