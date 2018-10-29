using System;
using System.Dynamic;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class MappingToDateTimeTests
    {
        public class PersonWithFields
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public DateTime StartDate;
            public DateTime? EndDate;
        }

        public class PersonWithProperties
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        [Fact]
        public void Can_Map_DateTime_Values_To_Nullable_DateTime_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var startDate = DateTime.Now.AddDays(-2);
            var endDate = DateTime.Now;

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.StartDate = startDate;
            dynamicPerson.EndDate = endDate;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.StartDate, startDate);
            Assert.Equal(customer.EndDate, endDate);
        }

        [Fact]
        public void Can_Map_DateTime_Values_To_Nullable_DateTime_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var startDate = DateTime.Now.AddDays(-2);
            var endDate = DateTime.Now; // This is what we are testing

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.StartDate = startDate;
            dynamicPerson.EndDate = endDate;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.StartDate, startDate);
            Assert.Equal(customer.EndDate, endDate); // This is what we are testing
        }

        [Fact]
        public void Can_Map_Null_Values_To_Nullable_DateTime_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var startDate = DateTime.Now.AddDays(-2);
            DateTime? endDate = null; // This is what we are testing

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.StartDate = startDate;
            dynamicPerson.EndDate = endDate;

            var mappy = new Mappy();

            // Act
            PersonWithFields customer = mappy.Map<PersonWithFields>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.StartDate, startDate);
            Assert.Equal(customer.EndDate, endDate); // This is what we are testing
        }

        [Fact]
        public void Can_Map_Null_Values_To_Nullable_DateTime_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            var startDate = DateTime.Now.AddDays(-2);
            DateTime? endDate = null; // This is what we are testing

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.StartDate = startDate;
            dynamicPerson.EndDate = endDate;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.StartDate, startDate);
            Assert.Equal(customer.EndDate, endDate); // This is what we are testing
        }

        [Fact]
        public void Can_Map_DateTime_String_Values_To_Nullable_DateTime_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            DateTime startDate = DateTime.Now.AddDays(-2);
            DateTime? endDate = DateTime.Now;

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.StartDate = startDate.ToString();
            dynamicPerson.EndDate = endDate.ToString();

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.StartDate.ToString(), startDate.ToString());
            Assert.Equal(customer.EndDate.ToString(), endDate.ToString());
        }

        [Fact]
        public void Can_Map_DateTime_String_Values_To_Nullable_DateTime_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Bob";
            const string lastName = "Smith";
            DateTime startDate = DateTime.Now.AddDays(-2);
            DateTime? endDate = DateTime.Now;

            dynamic dynamicPerson = new ExpandoObject();
            dynamicPerson.Id = id;
            dynamicPerson.FirstName = firstName;
            dynamicPerson.LastName = lastName;
            dynamicPerson.StartDate = startDate.ToString();
            dynamicPerson.EndDate = endDate.ToString();

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dynamicPerson);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.StartDate.ToString(), startDate.ToString());
            Assert.Equal(customer.EndDate.ToString(), endDate.ToString());
        }
    }
}