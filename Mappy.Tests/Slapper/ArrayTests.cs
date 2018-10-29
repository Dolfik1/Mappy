using System.Collections.Generic;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class ArrayTests
    {
        public class PersonWithFields
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public string[] FavoriteFoods;
        }

        public class PersonWithProperties
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string[] FavoriteFoods { get; set; }
        }

        [Fact]
        public void Can_Map_Null_Values_To_Null_Arrays()
        {
            // Arrange
            const int id = 1;
            const string firstName = null;
            const string lastName = "Smith";
            const string[] favoriteFoods = null;

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", null},
                {"LastName", lastName},
                {"FavoriteFoods", favoriteFoods}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Null(customer.FavoriteFoods);
        }

        [Fact]
        public void Can_Map_Array_Values_To_Arrays()
        {
            // Arrange
            const int id = 1;
            const string firstName = null;
            const string lastName = "Smith";
            string[] favoriteFoods = new[] { "Ice Cream", "Jello" };

            var dictionary = new Dictionary<string, object>
            {
                {"Id", id},
                {"FirstName", null},
                {"LastName", lastName},
                {"FavoriteFoods", favoriteFoods}
            };

            var mappy = new Mappy();

            // Act
            var customer = mappy.Map<PersonWithFields>(dictionary);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, id);
            Assert.Equal(customer.FirstName, firstName);
            Assert.Equal(customer.LastName, lastName);
            Assert.Equal(customer.FavoriteFoods, favoriteFoods);
        }
    }
}