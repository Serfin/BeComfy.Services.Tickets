using System;
using BeComfy.Common.CqrsFlow;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.Messages.Events
{
    public class BuyTicketRejected : IRejectedEvent
    {
        public Guid TicketId { get; }
        public Guid CustomerId { get; set; }
        public string Code { get; }
        public string Reason { get; }

        [JsonConstructor]
        public BuyTicketRejected(Guid ticketId, Guid customerId, string code, string reason)
        {
            TicketId = ticketId;
            CustomerId = customerId;
            Code = code;
            Reason = reason;
        }
    }
}