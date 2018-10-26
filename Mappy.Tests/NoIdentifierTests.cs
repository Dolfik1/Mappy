using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class NoIdentifierTests
    {
        public class PersonWithFields
        {
            public string FirstName;
            public string LastName;
        }

        [Fact]
        public void Can_Map_To_Types_With_No_Identifiers()
        {
            // Arrange
            const string person1FirstName = "Bob";
            const string person1LastName = "Smith";
            const string person2FirstName = "Nancy";
            const string person2LastName = "Sue";

            dynamic person1 = new ExpandoObject();
            person1.FirstName = person1FirstName;
            person1.LastName = person1LastName;

            dynamic person2 = new ExpandoObject();
            person2.FirstName = person2FirstName;
            person2.LastName = person2LastName;

            var list = new List<dynamic> {person1, person2};

            var mappy = new Mappy();

            // Act
            var persons = mappy.Map<PersonWithFields>(list)
                .ToList();

            // Assert
            Assert.NotNull(persons);
            Assert.Equal(2, persons.Count);
            Assert.Equal(persons[0].FirstName, person1FirstName);
            Assert.Equal(persons[0].LastName, person1LastName);
            Assert.Equal(persons[1].FirstName, person2FirstName);
            Assert.Equal(persons[1].LastName, person2LastName);
        }
    }
}