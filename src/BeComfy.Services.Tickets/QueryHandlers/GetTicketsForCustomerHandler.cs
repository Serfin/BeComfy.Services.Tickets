using System.Collections.Generic;
using System.Threading.Tasks;
using BeComfy.Common.CqrsFlow.Handlers;
using BeComfy.Services.Tickets.Dto;
using BeComfy.Services.Tickets.Queries;
using BeComfy.Services.Tickets.Repositories;

namespace BeComfy.Services.Tickets.QueryHandlers
{
    public class GetTicketsForCustomerHandler
        : IQueryHandler<GetTicketsForCustomer, IEnumerable<TicketDto>>
    {
        private readonly ITicketsRepository _ticketsRepository;

        public GetTicketsForCustomerHandler(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        public async Task<IEnumerable<TicketDto>> HandleAsync(GetTicketsForCustomer query)
        {
            var tickets = await _ticketsRepository.BrowseAsync(query.PageSize, query.Page, 
                x => x.Owner == query.CustomerId);

            var output = new List<TicketDto>();
            foreach (var ticket in tickets)
            {
                output.Add(new TicketDto()
                {
                    Owner = ticket.Owner,
                    FlightId = ticket.FlightId,
                    Seats = ticket.Seats,
                    TotalCost = ticket.TotalCost,
                    CreatedAt = ticket.CreatedAt
                });
            }

            return output;
        }
    }
}