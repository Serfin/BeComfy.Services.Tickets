using System;
using BeComfy.Common.CqrsFlow;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.Messages.Events
{
    public class BuyTicketRejected : IRejectedEvent
    {
        public Guid TicketId { get; }
        public string Code { get; }
        public string Reason { get; }

        [JsonConstructor]
        public BuyTicketRejected(Guid ticketId, string code, string reason)
        {
            TicketId = ticketId;
            Code = code;
            Reason = reason;
        }
    }
}