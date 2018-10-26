using System.Collections.Generic;

namespace Mappy.Benchmark
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
}
