using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class AbbreviationsTest
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
        public void Should_Map_Simple_Abbreviations_Correct()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "FullName", "Bob Smith" },
                { "Ph_Id", 1 },
                { "Ph_PhoneNumber", "1234" }
            };

            var data = new List<Dictionary<string, object>>
            {
                dictionary
            };

            var mappy = new Mappy();
            var customers = mappy.Map<Customer>(data, new Dictionary<string, string>
            {
                { "Phone_", "Ph_" }
            });

            Assert.Single(customers);
            var customer = customers.Single();

            Assert.Equal(dictionary["Id"], customer.Id);
            Assert.Equal(dictionary["FullName"], customer.FullName);

            Assert.NotNull(customer.Phone);

            Assert.Equal(dictionary["Ph_Id"], customer.Phone.Id);
            Assert.Equal(dictionary["Ph_PhoneNumber"], customer.Phone.PhoneNumber);

        }
    }
}
