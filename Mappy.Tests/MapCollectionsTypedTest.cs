using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class MapCollectionsTypedTest
    {
        public class Customer
        {
            public int CustomerId;
            public IList<int> OrdersIds;
        }

        public class CustomerNames
        {
            public int CustomerNamesId;
            public IList<string> Names;
        }

        [Fact]
        public void I_Can_Map_Value_PrimitiveTyped_Collection()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
                         {
                             { "CustomerId", 1 },
                             { "OrdersIds_$", 3 },
                         };

            var dictionary2 = new Dictionary<string, object>
                         {
                             { "CustomerId", 1 },
                             { "OrdersIds_$", 5 }
                         };

            var list = new List<IDictionary<string, object>> { dictionary, dictionary2 };

            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<Customer>(list)
                .ToList();

            // Assert

            // There should only be a single customer
            Assert.Single(customers);

            // There should be two values in OrdersIds, with the correct values
            Assert.Equal(2, customers.Single().OrdersIds.Count);
            Assert.Equal(3, customers.Single().OrdersIds[0]);
            Assert.Equal(5, customers.Single().OrdersIds[1]);
        }


        [Fact]
        public void I_Can_Map_Value_SpecialStringTyped_Collection()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
                         {
                             { "CustomerNamesId", 1 },
                             { "Names_$", "Name 1" },
                         };

            var dictionary2 = new Dictionary<string, object>
                         {
                             { "CustomerNamesId", 1 },
                             { "Names_$", "Name 2" }
                         };

            var list = new List<IDictionary<string, object>> { dictionary, dictionary2 };

            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<CustomerNames>(list).ToList();

            // Assert

            // There should only be a single customer
            Assert.Single(customers);

            // There should be two values in OrdersIds, with the correct values
            Assert.Equal(2, customers.Single().Names.Count);
            Assert.Equal("Name 1", customers.Single().Names[0]);
            Assert.Equal("Name 2", customers.Single().Names[1]);
        }

        public class Merchant
        {
            public Merchant()
            {
                Addresses = new HashSet<MerchantAddress>();
            }

            public long Id { set; get; }
            public string Name { set; get; }

            public ICollection<MerchantAddress> Addresses { set; get; }
        }

        public class MerchantAddress
        {
            public long Id { set; get; }
            public string AddressLine { set; get; }
            public long MerchantId { set; get; }
        }

        [Fact]
        public void I_Can_Map_Any_Typed_ICollection()
        {
            // this strings was received from database (or another flat storage)
            List<Dictionary<string, object>> flat = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    { "Id", 1 } ,
                    {"Name", "Merchant name" } ,
                    { "Addresses_Id", 1} ,
                    { "Addresses_AddressLine", "Address line 1"} ,
                    { "Addresses_MerchantId", 1}
                },
                new Dictionary<string, object>()
                {
                    { "Id", 1 } ,
                    {"Name", "Merchant name" } ,
                    { "Addresses_Id", 2} ,
                    { "Addresses_AddressLine", "Address line 2"} ,
                    { "Addresses_MerchantId", 1}
                },
                new Dictionary<string, object>()
                {
                    { "Id", 1 } ,
                    {"Name", "Merchant name" } ,
                    { "Addresses_Id", 3} ,
                    { "Addresses_AddressLine", "Address line 3"} ,
                    { "Addresses_MerchantId", 1}
                },
            };

            var mappy = new Mappy();

            var result = mappy.Map<Merchant>(flat);
            Assert.Single(result);
            var merchant = result.First();
            Assert.Equal(3, merchant.Addresses.Count);
            Assert.Equal("Address line 1", merchant.Addresses.First().AddressLine);
            Assert.Equal("Address line 2", merchant.Addresses.Skip(1).First().AddressLine);
            Assert.Equal("Address line 3", merchant.Addresses.Skip(2).First().AddressLine);
        }

    }
}
