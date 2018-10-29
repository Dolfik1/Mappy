using System;
using System.Collections.Generic;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class ExceptionTests
    {
        public class Person
        {
            public int PersonId;
            public string FirstName;
            public string LastName;
        }

        [Fact]
        public void Will_Throw_An_Exception_If_The_Type_Is_Not_Dynamic()
        {
            // Arrange
            var someObject = new object();

            var mappy = new Mappy();

            // Assert
            Assert.Throws<ArgumentException>(() => mappy.Map<Person>(new List<dynamic>() {new object()}));
        }

        [Fact]
        public void Will_Not_Throw_An_Exception_If_The_List_Items_Are_Not_Dynamic()
        {
            // Arrange
            var someObjectList = new List<object> {null};
            var mappy = new Mappy();

            // Act
            mappy.Map<Person>(someObjectList);
        }

        [Fact]
        public void Will_Return_An_Empty_List_Of_The_Requested_Type_When_Passed_An_Empty_List()
        {
            var mappy = new Mappy();

            // Act
            var list = mappy.Map<Person>(new List<object>());

            // Assert
            Assert.NotNull(list);
            Assert.Empty(list);
        }
    }
}