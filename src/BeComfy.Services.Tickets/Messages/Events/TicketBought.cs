using System;
using System.Collections.Generic;
using BeComfy.Common.CqrsFlow;
using BeComfy.Common.Types.Enums;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.Messages.Events
{
    public class TicketBought : IEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public Guid FlightId { get; }
        public decimal TotalPrice { get; }
        public IDictionary<SeatClass, int> AvailableSeats { get; }

        [JsonConstructor]
        public TicketBought(Guid id, Guid customerId, Guid flightId, decimal totalPrice,
            IDictionary<SeatClass, int> availableSeats)
        {
            Id = id;
            CustomerId = customerId;
            FlightId = flightId;
            TotalPrice = totalPrice;
            AvailableSeats = availableSeats;
        }
    }
}