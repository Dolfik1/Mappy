using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Mappy.Tests
{
    public class SimpleTypeConversionTests
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
        public void Can_Map_Matching_Field_Names_With_Different_Types()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";

            var dictionary = new Dictionary<string, object>
            {
                {"Id", double.Parse("1.245698", CultureInfo.InvariantCulture)},
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
        public void Can_Map_Matching_Property_Names_With_Different_Types()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";

            var dictionary = new Dictionary<string, object>
            {
                {"Id", Double.Parse("1.245698", CultureInfo.InvariantCulture)},
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