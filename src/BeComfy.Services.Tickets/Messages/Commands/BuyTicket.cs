using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using BeComfy.Common.CqrsFlow;
using BeComfy.Common.Types.Enums;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.Messages.Commands
{
    public class BuyTicket : ICommand
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public Guid FlightId { get; }
        public IDictionary<SeatClass, int> Seats { get; }
        
        [JsonConstructor]
        public BuyTicket(Guid id, Guid customerId, Guid flightId, IDictionary<SeatClass, int> seats)
        {
            Id = id;
            CustomerId = customerId;
            FlightId = flightId;
            Seats = seats;
        }   
    }
}