using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using TestDynamodb.Repositories;

namespace TestDynamodb.Models
{
    [DynamoDBTable(RegisterTables.TABLE_NAME_COMPANY)]
    public class Company
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        public List<Partner> Partners { get; set; } = new List<Partner>();
        public List<Phone> Phones { get; set; } = new List<Phone>();
        public List<Address> Addresses { get; set; } = new List<Address>();
    }

    public class Partner
    {
        public string Name { get; set; }
        public List<Phone> Phones { get; set; } = new List<Phone>();
        public List<Address> Addresses { get; set; } = new List<Address>();
    }
}
