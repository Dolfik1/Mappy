using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class CompatibilityTests
    {
        class Customer
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public Phone Phone { get; set; }
        }

        class Phone
        {
            public int Id { get; set; }
            public string PhoneNumber { get; set; }
        }

        [Fact]
        public void Should_Map_First_Not_Null_Items_In_Group()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "FullName", "Bob Smith" },
                { "Phone_Id", null },
                { "Phone_PhoneNumber", null }
            };

            var dictionary2 = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "FullName", "Bob Smith" },
                { "Phone_Id", 1 },
                { "Phone_PhoneNumber", "1234" }
            };

            var data = new List<Dictionary<string, object>>
            {
                dictionary,
                dictionary2
            };

            var mappy = new Mappy();
            var customers = mappy.Map<Customer>(data);

            Assert.Single(customers);
            var customer = customers.Single();

            Assert.Equal(dictionary["Id"], customer.Id);
            Assert.Equal(dictionary["FullName"], customer.FullName);

            Assert.NotNull(customer.Phone);

            Assert.Equal(dictionary2["Phone_Id"], customer.Phone.Id);
            Assert.Equal(dictionary2["Phone_PhoneNumber"], customer.Phone.PhoneNumber);
        }

        class Item
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public List<Item> Subitems { get; set; }
        }

        [Fact]
        public void Should_Use_Last_Item_In_Array()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "Value", "Test1" },
                { "Subitems_Id", 1 },
                { "Subitems_Value", "Test1" },
            };

            var dictionary2 = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "Value", "Test1" },
                { "Subitems_Id", 1 },
                { "Subitems_Value", "Test2" },
            };

            var data = new List<Dictionary<string, object>>
            {
                dictionary,
                dictionary2
            };

            var mappy = new Mappy();

            var items = mappy.Map<Item>(data);

            Assert.Single(items);
            var item = items.Single();

            Assert.Equal(dictionary["Id"], item.Id);
            Assert.Equal(dictionary["Value"], item.Value);

            Assert.Single(item.Subitems);
            var subitem = item.Subitems.Single();

            Assert.Equal(dictionary2["Subitems_Id"], subitem.Id);
            Assert.Equal(dictionary2["Subitems_Value"], subitem.Value);
        }
    }
}
