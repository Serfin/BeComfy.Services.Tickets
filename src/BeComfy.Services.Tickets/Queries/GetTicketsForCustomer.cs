using System;
using System.Collections.Generic;
using BeComfy.Common.CqrsFlow;
using BeComfy.Common.Mongo;
using BeComfy.Services.Tickets.Dto;

namespace BeComfy.Services.Tickets.Queries
{
    public class GetTicketsForCustomer : IQuery<IEnumerable<TicketDto>>, IPaginable
    {
        public Guid CustomerId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}