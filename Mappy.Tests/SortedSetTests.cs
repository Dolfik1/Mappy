using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class SortedSetTests
    {        
        class Customer
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public SortedSet<Order> Orders { get; set; }
        }

        class Order
        {
            public int OrderId { get; set; }
            public decimal OrderTotal { get; set; }
        }

        [Fact]
        public void Can_Map_To_SortedSet()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_OrderId", 1},
                {"Orders_OrderTotal", 50.50m}
            };


            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<Customer>(dictionary);

            // Assert

            Assert.Equal(dictionary["CustomerId"], customer.CustomerId);
            Assert.Equal(dictionary["FirstName"], customer.FirstName);
            Assert.Equal(dictionary["LastName"], customer.LastName);

            Assert.Single(customer.Orders);
            Assert.Equal(dictionary["Orders_OrderId"], customer.Orders.Single().OrderId);
            Assert.Equal(dictionary["Orders_OrderTotal"], customer.Orders.Single().OrderTotal);
        }       
        
        [Fact]
        public void Should_Map_Not_Exists_Complex_Array_Values_To_NotNull_SortedSet()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<Customer>(dictionary);

            // Assert

            Assert.Equal(dictionary["CustomerId"], customer.CustomerId);
            Assert.Equal(dictionary["FirstName"], customer.FirstName);
            Assert.Equal(dictionary["LastName"], customer.LastName);

            Assert.NotNull(customer.Orders);
            Assert.Empty(customer.Orders);
        }
    }
}
