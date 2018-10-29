using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class MapDynamicTests
    {
        public class Customer
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public ICollection<Order> Orders;
        }

        public class Order
        {
            public int Id;
            public decimal OrderTotal;
        }

        public class CustomerSingleOrder
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public SingleOrder Order;
        }

        public class SingleOrder
        {
            public int Id;
            public OrderDetails Details;
        }

        public class OrderDetails
        {
            public string Address;
        }

        [Fact]
        public void Can_Handle_Mapping_A_Single_Dynamic_Object()
        {
            // Arrange
            dynamic dynamicCustomer = new ExpandoObject();
            dynamicCustomer.Id = 1;
            dynamicCustomer.FirstName = "Bob";
            dynamicCustomer.LastName = "Smith";
            dynamicCustomer.Orders_Id = 1;
            dynamicCustomer.Orders_OrderTotal = 50.50m;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<Customer>(dynamicCustomer) as Customer;

            // Assert
            Assert.NotNull(customer);
            Assert.Single(customer.Orders);
        }

        [Fact]
        public void Can_Handle_Mapping_Nested_Members_Using_Dynamic()
        {
            // Arrange
            var dynamicCustomers = new List<object>();

            for (var i = 0; i < 5; i++)
            {
                dynamic customer = new ExpandoObject();
                customer.Id = i;
                customer.FirstName = "FirstName" + i;
                customer.LastName = "LastName" + i;
                customer.Orders_Id = i;
                customer.Orders_OrderTotal = i + 0m;

                dynamicCustomers.Add(customer);
            }

            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<Customer>(dynamicCustomers)
                .ToList();

            // Assert
            Assert.Equal(5, customers.Count());
            Assert.Equal(1, customers.First().Orders.Count);
        }

        [Fact]
        public void Nested_Member_Should_Be_Null_If_All_Values_Are_Null()
        {
            // Arrange
            dynamic customer = new ExpandoObject();
            customer.Id = 1;
            customer.FirstName = "FirstName";
            customer.LastName = "LastName";
            customer.Order_Id = null;
            customer.Order_OrderTotal = null;

            var mappy = new Mappy();

            // Act
            var test = mappy.Map<CustomerSingleOrder>(customer);

            // Assert
            Assert.NotNull(test);
            Assert.Null(test.Order);
        }

        [Fact]
        public void Nested_Member_Should_Be_Null_Only_If_All_Nested_Values_Are_Null()
        {
            // Arrange
            dynamic customer = new ExpandoObject();
            customer.Id = 1;
            customer.FirstName = "FirstName";
            customer.LastName = "LastName";
            customer.Order_Id = null;
            customer.Order_OrderTotal = null;
            customer.Order_Details_Address = "123 Fake Ave.";

            var mappy = new Mappy();

            // Act
            var test = mappy.Map<CustomerSingleOrder>(customer);

            // Assert
            Assert.NotNull(test);
            Assert.NotNull(test.Order);
            Assert.NotNull(test.Order.Details);
            Assert.True(!string.IsNullOrWhiteSpace(test.Order.Details.Address));
            Assert.Equal(test.Order.Details.Address, "123 Fake Ave.");
        }
    }
}