using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests.Slapper
{
    public class ComplexMapsParentsAndChlidTest
    {
        public class Hotel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Tour
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Service
        {
            public int Id { get; set; }
            public IEnumerable<Hotel> Hotels { get; set; }
            public IEnumerable<Tour> Tours { get; set; }
        }

        public class Booking
        {
            public int Id { get; set; }
            public IEnumerable<Service> Services { get; set; }
        }

        [Fact]
        public void Can_Make_Cache_HashTypeEquals_With_Different_Parents()
        {
            var listOfDictionaries = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Id", 1},
                    {"Services_Id", 1},
                    {"Services_Hotels_Id", 1},
                    {"Services_Hotels_Name", "Hotel 1"}
                },
                new Dictionary<string, object>
                {
                    {"Id", 1},
                    {"Services_Id", 2},
                    {"Services_Hotels_Id", 2},
                    {"Services_Hotels_Name", "Hotel 2"}
                },
                new Dictionary<string, object>
                {
                    {"Id", 2},
                    {"Services_Id", 1},
                    {"Services_Hotels_Id", 3},
                    {"Services_Hotels_Name", "Hotel 3"}
                }
            };

            var mappy = new Mappy();

            var bookings = mappy.Map<Booking>(listOfDictionaries)
                .ToList();

            Assert.Equal(2, bookings.Count);
            Assert.Equal(2, bookings[0].Services.Count());

            Assert.NotNull(bookings[0].Services.SingleOrDefault(s => s.Id == 1));
            Assert.Single(bookings[0].Services.Single(s => s.Id == 1).Hotels);
            Assert.Single(bookings[0].Services.Single(s => s.Id == 2).Hotels);

            Assert.Single(bookings[1].Services);

            Assert.NotNull(bookings[1].Services.SingleOrDefault(s => s.Id == 1));
            Assert.Single(bookings[1].Services.Single(s => s.Id == 1).Hotels);
        }
    }
}