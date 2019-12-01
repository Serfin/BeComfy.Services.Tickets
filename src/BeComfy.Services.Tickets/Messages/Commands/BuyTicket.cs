using System;
using System.Collections.Generic;
using BeComfy.Common.CqrsFlow;
using BeComfy.Common.Types.Enums;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.Messages.Commands
{
    public class BuyTicket : ICommand
    {
        public Guid FlightId { get; }
        public Guid CustomerId { get; }
        public IDictionary<SeatClass, int> Seats { get; }
        
        [JsonConstructor]
        public BuyTicket(Guid flightId, Guid customerId, IDictionary<SeatClass, int> seats)
        {
            FlightId = flightId;
            CustomerId = customerId;
            Seats = seats;
        }   
    }
}