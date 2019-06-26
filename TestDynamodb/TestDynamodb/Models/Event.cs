using System;

namespace TestDynamodb.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Visibility { get; set; }
        public object Data { get; set; }
    }
}
