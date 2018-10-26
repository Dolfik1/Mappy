using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace Mappy.Tests
{
    public class EmptyListe
    {
        public class Customer
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public IList<Order> Orders;
        }

        public class Order
        {
            public int Id;
            public decimal OrderTotal;
        }

        [Fact]
        public void Can_Handle_Mapping_An_Empty_List()
        {
            // Arrange
            dynamic dynamicCustomer = new ExpandoObject();
            dynamicCustomer.Id = 1;
            dynamicCustomer.FirstName = "Bob";
            dynamicCustomer.LastName = "Smith";
            dynamicCustomer.Orders_Id = null;
            dynamicCustomer.Orders_OrderTotal = null;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<Customer>(dynamicCustomer) as Customer;

            // Assert
            Assert.NotNull(customer);

            // Empty list
            Assert.NotNull(customer.Orders);
            Assert.Equal(0, customer.Orders.Count);
        }
    }
}