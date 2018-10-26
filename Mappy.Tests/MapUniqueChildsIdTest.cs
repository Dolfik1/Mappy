using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class MapUniqueChildsIdTest
    {
        public class NameValue
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public IEnumerable<NameValue> Phones { get; set; }
            public IEnumerable<NameValue> Emails { get; set; }
        }

        [Fact]
        public void Can_Map_DifferentsRows_to_Same_object()
        {
            dynamic dynamicCustomer = new ExpandoObject();
            dynamicCustomer.Id = 1;
            dynamicCustomer.Name = "Clark";
            dynamicCustomer.Phones_Id = 1;
            dynamicCustomer.Phones_Name = "88888";
            dynamicCustomer.Emails_Id = "1";
            dynamicCustomer.Emails_Name = "a@b.com";

            dynamic dynamicCustomer2 = new ExpandoObject();
            dynamicCustomer2.Id = 1;
            dynamicCustomer2.Name = "Clark";
            dynamicCustomer2.Phones_Id = 2;
            dynamicCustomer2.Phones_Name = "99999";
            dynamicCustomer2.Emails_Id = "2";
            dynamicCustomer2.Emails_Name = "c@c.com";

            var list = new List<dynamic> { dynamicCustomer, dynamicCustomer2 };

            var mappy = new Mappy();
            var customer = mappy.Map<NameValue>(list)
                .FirstOrDefault();

            Assert.NotNull(customer);
            Assert.NotEqual(
                customer.Emails.FirstOrDefault(e => e.Id == 1).Name,
                customer.Phones.FirstOrDefault(p => p.Id == 1).Name);
        }
    }
}
