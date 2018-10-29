using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class ComplexMapTests
    {
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

        public static class MapTestModels
        {
            public class CustomerWithMultipleIdAttributes
            {
                [Id] public int Customer_Id;

                [Id] public string Customer_Type;

                public string FirstName;

                public string LastName;

                public List<Order> Orders;
            }

            public class CustomerWithOrdersList
            {
                public int Id;
                public string FirstName;
                public string LastName;
                public List<Order> Orders;
            }

            public class CustomerWithAnIEnumerableOrdersCollection
            {
                public int Id;
                public string FirstName;
                public string LastName;
                public IEnumerable<Order> Orders;
            }

            public class Order
            {
                public int Id;
                public decimal OrderTotal;
                public IList<OrderDetail> OrderDetails;
            }

            public class OrderDetail
            {
                public int Id;
                public decimal OrderDetailTotal;
                public Product Product;
            }

            public class Product
            {
                public int Id;
                public string ProductName;
            }
        }

        [Fact]
        public void Can_Map_Complex_Nested_Members()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            const int orderId = 1;
            const decimal orderTotal = 50.50m;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId},
                {"Orders_OrderTotal", orderTotal}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<MapTestModels.CustomerWithOrdersList>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.NotNull(customer.Orders);
            Assert.Single(customer.Orders);
            Assert.Equal(customer.Orders[0].Id, orderId);
            Assert.Equal(customer.Orders[0].OrderTotal, orderTotal);
        }

        /// <summary>
        /// OLD SUMMARY ===
        /// When mapping, it internally keeps a cache of instantiated objects with the key being the
        /// hash of the objects identifier hashes summed together so when another record with the exact
        /// same identifier hash is detected, it will re-use the existing instantiated object instead of
        /// creating a second one alleviating the burden of the consumer of the library to group objects
        /// by their identifier.
        /// ===
        /// This was flawed as SAME HASHCODE DOESN'T MEAN SAME VALUE. Hash collisions would lead to
        /// wrongly reusing an instance instead of creating a new one (issue #48).
        /// It's now fixed as real identifier values are compared, not their hashes anymore.
        /// </summary>
        [Fact]
        public void Can_Detect_Duplicate_Parent_Members_And_Properly_Instantiate_The_Object_Only_Once()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            const int orderId = 1;
            const decimal orderTotal = 50.50m;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId},
                {"Orders_OrderTotal", orderTotal}
            };

            var dictionary2 = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId + 1},
                {"Orders_OrderTotal", orderTotal + 1}
            };

            var listOfDictionaries = new List<Dictionary<string, object>> {dictionary, dictionary2};

            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<MapTestModels.CustomerWithOrdersList>(listOfDictionaries);

            var customer = customers.FirstOrDefault();

            // Assert
            Assert.Single(customers);
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.NotNull(customer.Orders);
            Assert.Equal(2, customer.Orders.Count);
            Assert.Equal(customer.Orders[0].Id, orderId);
            Assert.Equal(customer.Orders[0].OrderTotal, orderTotal);
            Assert.Equal(customer.Orders[1].Id, orderId + 1);
            Assert.Equal(customer.Orders[1].OrderTotal, orderTotal + 1);
        }

        [Fact]
        public void Can_Handle_Nested_Members_That_Implements_ICollection()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            const int orderId = 1;
            const decimal orderTotal = 50.50m;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId},
                {"Orders_OrderTotal", orderTotal}
            };

            var dictionary2 = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId + 1},
                {"Orders_OrderTotal", orderTotal + 1}
            };

            var listOfDictionaries = new List<Dictionary<string, object>> {dictionary, dictionary2};

            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<MapTestModels.CustomerWithAnIEnumerableOrdersCollection>(listOfDictionaries);

            var customer = customers.FirstOrDefault();

            // Assert
            Assert.Equal(2, customer.Orders.Count());
        }

        [Fact]
        public void Can_Handle_Mapping_Objects_With_Multiple_Identifiers()
        {
            // Arrange
            const int customerId = 1;
            const string customerType = "Commercial";
            const string firstName = "Bob";
            const string lastName = "Smith";
            const int orderId = 1;
            const decimal orderTotal = 50.50m;

            var dictionary = new Dictionary<string, object>
            {
                {"Customer_Id", customerId},
                {"Customer_Type", customerType},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId},
                {"Orders_OrderTotal", orderTotal}
            };

            var dictionary2 = new Dictionary<string, object>
            {
                {"Customer_Id", customerId},
                {"Customer_Type", customerType},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId + 1},
                {"Orders_OrderTotal", orderTotal + 1}
            };

            var dictionary3 = new Dictionary<string, object>
            {
                {"Customer_Id", customerId + 1},
                {"Customer_Type", customerType},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Orders_Id", orderId + 1},
                {"Orders_OrderTotal", orderTotal + 1}
            };

            var listOfDictionaries = new List<Dictionary<string, object>> {dictionary, dictionary2, dictionary3};

            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<MapTestModels.CustomerWithMultipleIdAttributes>(listOfDictionaries);

            // Assert
            Assert.Equal(2, customers.Count());
            Assert.Equal(2, customers.First().Orders.Count);
            Assert.Equal(customers.ToList()[1].Orders[0].Id, orderId + 1);
        }

        [Fact]
        public void Can_Map_To_Multiple_Objects()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"Id", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_Id", 1},
                {"Orders_OrderTotal", 50.50m}
            };

            var dictionary2 = new Dictionary<string, object>
            {
                {"Id", 2},
                {"FirstName", "Jane"},
                {"LastName", "Doe"},
                {"Orders_Id", 2},
                {"Orders_OrderTotal", 23.40m}
            };

            var listOfDictionaries = new List<Dictionary<string, object>> {dictionary, dictionary2};

            var mappy = new Mappy();
            // Act
            var customers = mappy.Map<MapTestModels.CustomerWithAnIEnumerableOrdersCollection>(listOfDictionaries);

            // Assert
            Assert.Equal(2, customers.Count());
            Assert.Equal("Bob", customers.ToList()[0].FirstName);
            Assert.Equal("Jane", customers.ToList()[1].FirstName);
        }

        [Fact]
        public void Can_Handle_Mapping_Deeply_Nested_Members()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"Id", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"},
                {"Orders_Id", 1},
                {"Orders_OrderTotal", 50.50m},
                {"Orders_OrderDetails_Id", 1},
                {"Orders_OrderDetails_OrderDetailTotal", 50.50m},
                {"Orders_OrderDetails_Product_Id", 546},
                {"Orders_OrderDetails_Product_ProductName", "Black Bookshelf"}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<MapTestModels.CustomerWithAnIEnumerableOrdersCollection>(dictionary);

            // Assert
            Assert.Single(customer.Orders);
            Assert.Single(customer.Orders.First().OrderDetails);
            Assert.Equal("Black Bookshelf", customer.Orders.First().OrderDetails[0].Product.ProductName);
        }

        [Fact]
        public void Can_Handle_Resolving_Duplicate_Nested_Members()
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
            var customers = mappy.Map<Customer>(list);

            // Assert
            Assert.Single(customers);

            // We should only have one Order object
            Assert.Equal(1, customers.FirstOrDefault()?.Orders.Count);

            // We should only have one Order object and two OrderDetail objects
            Assert.Equal(2, customers.FirstOrDefault().Orders.FirstOrDefault().OrderDetails.Count);
        }
    }
}