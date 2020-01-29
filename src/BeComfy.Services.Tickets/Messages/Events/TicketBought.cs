using System;
using BeComfy.Common.CqrsFlow;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.Messages.Events
{
    public class TicketBought : IEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public decimal TotalPrice { get; }

        [JsonConstructor]
        public TicketBought(Guid id, Guid customerId, decimal totalPrice)
        {
            Id = id;
            CustomerId = customerId;
            TotalPrice = totalPrice;
        }
    }
}