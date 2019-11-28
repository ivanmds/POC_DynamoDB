using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace TestDynamodb.Models
{
    [DynamoDBTable("Person")]
    public class Person
    {
        public Person() => Created = DateTime.Now;

        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        public List<Phone> Phones { get; set; } = new List<Phone>();
        public List<Address> Addresses { get; set; } = new List<Address>();
    }


    public class Phone
    {
        public string DDD { get; set; }
        public string Number { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
