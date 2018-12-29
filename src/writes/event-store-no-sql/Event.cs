using System;
using MongoDB.Bson;

namespace EventStore.NoSql
{
    /// <summary>
    /// Class describing a event document stored in a NoSQL database.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Gets or sets the version of the event.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the assembly qualified type name of the event.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the event body, i.e. the actual event published from the aggregate.
        /// </summary>
        public BsonDocument Body { get; set; }

        /// <summary>
        /// Gets or sets the time the event was saved to the database.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
