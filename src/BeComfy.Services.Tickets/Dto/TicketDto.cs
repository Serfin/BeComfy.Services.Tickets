using System;
using System.Collections.Generic;
using BeComfy.Common.Types.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace BeComfy.Services.Tickets.Dto
{
    public class TicketDto
    {
        public Guid FlightId { get; set; }
        public Guid Owner { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public IDictionary<SeatClass, int> Seats { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}