using System.Linq;
using TestDynamodb.Models;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System;

namespace TestDynamodb.Helpers
{
    public static class EventParse
    {
        private static IEnumerable<string> EVENT_PROPERTIES = new string[] { "Id", "ParentId", "Account", "CreatedAt", "Name", "Origin", "Visibility" };

        public static IEnumerable<Event> Parse(QueryResponse response)
        {
            foreach(var item in response.Items)
            {
                var eventProperties = item.Where(p => EVENT_PROPERTIES.Contains(p.Key));
                var dataProperties = item.Where(p => !EVENT_PROPERTIES.Contains(p.Key));

                Event @event = BuildObject.Get<Event>(eventProperties);

                object data = BuildObject.Get(dataProperties);

                @event.Data = data;


                yield return @event;
            }
        }
    }
}
