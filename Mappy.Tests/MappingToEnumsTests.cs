using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace Mappy.Tests
{
    public class MappingToEnumTests
    {
        public enum Gender
        {
            Female = 1,
            Male = 2
        }

        public enum MaritalStatus
        {
            Married = 1,
            Single = 2
        }

        public class PersonWithFields
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public Gender Gender;
            public MaritalStatus? MaritalStatus;
        }

        public class PersonWithProperties
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Gender Gender { get; set; }
            public MaritalStatus? MaritalStatus { get; set; }
        }

        [Fact]
        public void Can_Map_Enum_Values_To_Enum_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Gender", gender}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
        }

        [Fact]
        public void Can_Map_Enum_Values_To_Enum_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Gender", gender}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
        }

        [Fact]
        public void Can_Map_Integer_Values_To_Enum_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Gender", 2}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
        }

        [Fact]
        public void Can_Map_Integer_Values_To_Enum_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Gender", 2}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
        }

        [Fact]
        public void Can_Map_String_Values_To_Enum_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Gender", "2"}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
        }

        [Fact]
        public void Can_Map_String_Values_To_Enum_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", firstName},
                {"LastName", lastName},
                {"Gender", "2"}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
        }

        [Fact]
        public void Can_Map_Null_Values_To_Nullable_Enum_Fields()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;
            MaritalStatus? maritalStatus = null;

            dynamic person = new ExpandoObject();
            person.Id = id;
            person.FirstName = firstName;
            person.LastName = lastName;
            person.Gender = gender;
            person.MaritalStatus = maritalStatus;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(person);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
            Assert.Equal(maritalStatus, customer.MaritalStatus);
        }

        [Fact]
        public void Can_Map_Null_Values_To_Nullable_Enum_Properties()
        {
            // Arrange
            const int id = 1;
            const string firstName = "Jimbo";
            const string lastName = "Smith";
            const Gender gender = Gender.Male;
            MaritalStatus? maritalStatus = null;

            dynamic person = new ExpandoObject();
            person.Id = id;
            person.FirstName = firstName;
            person.LastName = lastName;
            person.Gender = gender;
            person.MaritalStatus = maritalStatus;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(person);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(id, customer.Id);
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal(gender, customer.Gender);
            Assert.Equal(maritalStatus, customer.MaritalStatus);
        }

        [Fact]
        public void Can_Map_Int32_Values_To_NUllable_Enum_Properties()
        {
            dynamic person = new ExpandoObject();

            person.Id = 1;
            person.FirstName = "FirstName";
            person.LastName = "LastName";
            person.MaritalStatus = 2;
            person.Gender = Gender.Male;

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithProperties>(person);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(MaritalStatus.Single, customer.MaritalStatus);
        }
    }
}