using System;
using System.Dynamic;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class MappingToGuidTests
    {
        public class PersonWithFields
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public Guid UniqueId;
            public Guid? ANullableUniqueId;
        }

        public class PersonWithProperties
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Guid UniqueId { get; set; }
            public Guid? ANullableUniqueId { get; set; }
        }

        [Fact]
        public void Can_Map_Guid_Values_To_Guid_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var uniqueId = Guid.NewGuid();

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.UniqueId = uniqueId;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(uniqueId, customer.UniqueId);
        }

        [Fact]
        public void Can_Map_Guid_Values_To_Guid_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var uniqueId = Guid.NewGuid();

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.UniqueId = uniqueId;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(uniqueId, customer.UniqueId);
        }

        [Fact]
        public void Can_Map_Guid_String_Values_To_Guid_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var uniqueId = Guid.NewGuid();

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.UniqueId = uniqueId.ToString(); // This is what we are testing

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(uniqueId, customer.UniqueId); // This is what we are testing
        }

        [Fact]
        public void Can_Map_Guid_String_Values_To_Guid_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var uniqueId = Guid.NewGuid();

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.UniqueId = uniqueId.ToString(); // This is what we are testing

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(uniqueId, customer.UniqueId); // This is what we are testing
        }

        [Fact]
        public void Can_Map_Null_Values_To_Guid_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            Guid uniqueId = Guid.NewGuid();
            Guid? aNullableUniqueId = null;

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.UniqueId = uniqueId.ToString();
            dynamicPerson.ANullableUniqueId = aNullableUniqueId; // This is what we are testing

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(uniqueId, customer.UniqueId);
            Assert.Equal(aNullableUniqueId, customer.ANullableUniqueId); // This is what we are testing
        }

        [Fact]
        public void Can_Map_Null_Values_To_Guid_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var uniqueId = Guid.NewGuid();
            Guid? aNullableUniqueId = null;

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.UniqueId = uniqueId.ToString();
            dynamicPerson.ANullableUniqueId = aNullableUniqueId; // This is what we are testing

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(uniqueId, customer.UniqueId);
            Assert.Equal(aNullableUniqueId, customer.ANullableUniqueId); // This is what we are testing
        }
    }
}