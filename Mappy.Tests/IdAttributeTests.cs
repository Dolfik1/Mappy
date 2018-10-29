using Mappy.Tests.Attributes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{

    public class IdAttributeTests
    {
        class User
        {
            [IdTest]
            public int SomeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [Fact]
        public void Can_Use_Custom_Identity_Attribute()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                {"SomeId", 1},
                {"FirstName", "Bob"},
                {"LastName", "Smith"}
            };

            var dictionary2 = new Dictionary<string, object>
            {
                {"SomeId", 2},
                {"FirstName", "Bob"},
                {"LastName", "Smith"}
            };


            var dictionary3 = new Dictionary<string, object>
            {
                {"SomeId", 2},
                {"FirstName", "Bob"},
                {"LastName", "Smith"}
            };

            var options = new MappyOptions();
            var mappy = new Mappy(options);

            var users = mappy.Map<User>(new List<Dictionary<string, object>>
            {
                dictionary,
                dictionary2
            }).ToList();

            Assert.Equal(2, users.Count);
            Assert.Equal(dictionary["SomeId"], users.First().SomeId);

        }
    }
}
