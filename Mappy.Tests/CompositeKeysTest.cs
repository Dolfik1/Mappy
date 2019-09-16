using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
    public class CompositeKeysTest
    {
        public class Customer
        {
            public int Id { get; set; }
            [Id]
            public int OtherId { get; set; }
        }

        [Fact]
        public void Composite_keys_should_map_correctly()
        {
            // Arrange
            var d1 = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "OtherId", 1}
            };
            
            var d2 = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "OtherId", 2}
            };
            
            var d3 = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "OtherId", 1}
            };

            var data = new List<Dictionary<string, object>>
            {
                d1, d2, d3
            };
            
            var mappy = new Mappy();

            // Act
            var customers = mappy.Map<Customer>(data)
                .ToList();

            // Assert
            Assert.Equal(2, customers.Count());
            Assert.Equal(1, customers[0].Id);
            Assert.Equal(2, customers[1].OtherId);
        }
    }
}