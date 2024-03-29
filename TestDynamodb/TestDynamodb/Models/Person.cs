﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using TestDynamodb.Repositories;

namespace TestDynamodb.Models
{
    [DynamoDBTable(RegisterTables.TABLE_NAME_PERSON)]
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
}
