using System.Collections.Generic;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class SimplyMapTests
    {
        public class PersonWithFields
        {
            public int Id;
            public string FirstName;
            public string LastName;
        }

        public class PersonWithProperties
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [Fact]
        public void Can_Map_Matching_Field_Names()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName}
            };

            var mappy = new Mappy();
            // Act
            var customer = mappy.Map<PersonWithFields>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
        }

        [Fact]
        public void Can_Map_Matching_Property_Names()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
        }
    }
}