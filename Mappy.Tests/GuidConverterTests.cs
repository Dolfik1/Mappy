using System;
using Mappy.Converters;
using Xunit;

namespace Mappy.Tests
{
    public class GuidConverterTests
    {
        private readonly GuidConverter _converter = new GuidConverter();

        [Fact]
        public void Can_Convert_To_Type()
        {
            // Act + Assert
            Assert.True(_converter.CanConvert<Guid>(null)); // Input value does not matter, null is enough for the test.
        }
    }
}