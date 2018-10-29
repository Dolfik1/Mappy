using System.Collections.Generic;
using Xunit;

namespace Mappy.Tests
{
    public class PropertiesTests
    {
        class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName => FirstName + " " + LastName;
        }

        [Fact]
        public void Set_Property_Should_Be_Ignored()
        {
            var dictionary = new Dictionary<string, object>
            {
                {"FirstName", "Bob"},
                {"LastName", "Smith"}
            };

            var mappy = new Mappy();

            // Act
            var user = mappy.Map<User>(dictionary);

            Assert.Equal(dictionary["FirstName"], user.FirstName);
            Assert.Equal(dictionary["LastName"], user.LastName);
        }
    }
}
