using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class ReadMeTests
    {
        public class Person
        {
            public int Id;
            public string FirstName;
            public string LastName;
        }

        public class Customer
        {
            public int CustomerId;
            public string FirstName;
            public string LastName;
            public IList<Order> Orders;
        }

        public class Order
        {
            public int OrderId;
            public decimal OrderTotal;
            public IList<OrderDetail> OrderDetails;
        }

        public class OrderDetail
        {
            public int OrderDetailId;
            public decimal OrderDetailTotal;
        }

        [Fact]
        public void I_Can_Map_Nested_Types_And_Resolve_Duplicate_Entries_Properly()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_OrderId", 1},
                {"Orders_OrderTotal", 50.50m},
                {"Orders_OrderDetails_OrderDetailId", 1},
                {"Orders_OrderDetails_OrderDetailTotal", 25.00m}
            };

            var dictionary2 = new Dictionary<string, object>
            {
                {"CustomerId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_OrderId", 1},
                {"Orders_OrderTotal", 50.50m},
                {"Orders_OrderDetails_OrderDetailId", 2},
                {"Orders_OrderDetails_OrderDetailTotal", 25.50m}
            };

            var list = new List<IDictionary<string, object>> {dictionary, dictionary2};

            var mappy = new Mappy();
            
            // Act
            var customers = mappy.Map<Customer>(list)
                .ToList();

            // Assert

            // There should only be a single customer
            Assert.Single(customers);

            // There should only be a single Order
            Assert.Single(customers.Single().Orders);

            // There should be two OrderDetails
            Assert.Equal(2, customers.Single().Orders.Single().OrderDetails.Count);
        }

        [Fact]
        public void I_Can_Map_Nested_Types_And_Resolve_Duplicate_Entries_Properly_Using_Dynamics()
        {
            // Arrange
            dynamic customer1 = new ExpandoObject();
            customer1.CustomerId = 1;
            customer1.FirstName = "Bob";
            customer1.LastName = "Smith";
            customer1.Orders_OrderId = 1;
            customer1.Orders_OrderTotal = 50.50m;
            customer1.Orders_OrderDetails_OrderDetailId = 1;
            customer1.Orders_OrderDetails_OrderDetailTotal = 25.00m;

            dynamic customer2 = new ExpandoObject();
            customer2.CustomerId = 1;
            customer2.FirstName = "Bob";
            customer2.LastName = "Smith";
            customer2.Orders_OrderId = 1;
            customer2.Orders_OrderTotal = 50.50m;
            customer2.Orders_OrderDetails_OrderDetailId = 2;
            customer2.Orders_OrderDetails_OrderDetailTotal = 25.50m;

            var customerList = new List<dynamic> {customer1, customer2};

            var mappy = new Mappy();
            
            // Act
            var customers = mappy.Map<Customer>(customerList)
                .ToList();

            // Assert

            // There should only be a single customer
            Assert.Single(customers);

            // There should only be a single Order
            Assert.Single(customers.Single().Orders);

            // There should be two OrderDetails
            Assert.Equal(2, customers.Single().Orders.Single().OrderDetails.Count);
        }

        [Fact]
        public void Can_Map_Matching_Field_Names_With_Ease()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"Id", 1},
                {"FirstName", "Clark"},
                {"LastName", "Kent"}
            };

            var mappy = new Mappy();
            
            // Act
            var person = mappy.Map<Person>(dictionary);

            // Assert
            Assert.NotNull(person);
            Assert.Equal(1, person.Id);
            Assert.Equal("Clark", person.FirstName);
            Assert.Equal("Kent", person.LastName);
        }

        [Fact]
        public void Can_Map_Matching_Field_Names_Using_Dynamic()
        {
            // Arrange
            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = 1;
            dynamicPerson.FirstName = "Clark";
            dynamicPerson.LastName = "Kent";

            var mappy = new Mappy();
            
            // Act
            var person = mappy.Map<Person>(dynamicPerson) as Person;

            // Assert
            Assert.NotNull(person);
            Assert.Equal(1, person.Id);
            Assert.Equal("Clark", person.FirstName);
            Assert.Equal("Kent", person.LastName);
        }
    }
}