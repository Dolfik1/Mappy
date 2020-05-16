using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mappy.Tests
{
  public class ConstructorsTests 
  {
      class Customer
      {
          public readonly int Id;
          public string FullName { get; }
          public Phone Phone { get; }

          public Customer(int id, string fullName, Phone phone)
          {
            Id = id;
            FullName = fullName;
            Phone = phone;
          }
      }

      class Phone
      {
          public int Id { get; }
          public string PhoneNumber { get; }

          public Phone(int id, string phoneNumber)
          {
            Id = id;
            PhoneNumber = phoneNumber;
          }
      }

      class Customer2
      {
          public int Id { get; }
          public string FullName { get; set; }

          public Customer2(int id)
          {
            Id = id;
          }
      }
      
      [Fact]
      public void Should_Map_Data_Objects()
      {
          var dictionary = new Dictionary<string, object>
          {
              { "Id", 1 },
              { "FullName", "Bob Smith" },
              { "Phone_Id", 1 },
              { "Phone_PhoneNumber", "1234" }
          };

          var data = new List<Dictionary<string, object>>
          {
            dictionary
          };

          var mappy = new Mappy();
          var customers = mappy.Map<Customer>(data);

          Assert.Single(customers);
          var customer = customers.Single();

          Assert.Equal(dictionary["Id"], customer.Id);
          Assert.Equal(dictionary["FullName"], customer.FullName);

          Assert.NotNull(customer.Phone);

          Assert.Equal(dictionary["Phone_Id"], customer.Phone.Id);
          Assert.Equal(dictionary["Phone_PhoneNumber"], customer.Phone.PhoneNumber);
      }
      
      [Fact]
      public void Should_Map_Partial_Data_Constructors()
      {
          var dictionary = new Dictionary<string, object>
          {
              { "Id", 1 },
              { "FullName", "Bob Smith" }
          };

          var data = new List<Dictionary<string, object>>
          {
            dictionary
          };

          var mappy = new Mappy();
          var customers = mappy.Map<Customer2>(data);

          Assert.Single(customers);
          var customer = customers.Single();

          Assert.Equal(dictionary["Id"], customer.Id);
          Assert.Equal(dictionary["FullName"], customer.FullName);
      }
  }
}