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
        public Guid OwnerData { get; }
        public IDictionary<SeatClass, int> Seats { get;}
        public decimal TotalPrice { get; }

        [JsonConstructor]
        public TicketBought(Guid id, Guid ownerData, IDictionary<SeatClass, int> seats, decimal totalPrice)
        {
            Id = id;
            OwnerData = ownerData;
            Seats = seats;
            TotalPrice = totalPrice;
        }
    }
}