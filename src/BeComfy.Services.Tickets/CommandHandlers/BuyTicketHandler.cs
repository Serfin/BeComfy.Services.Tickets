using System.Threading.Tasks;
using BeComfy.Common.CqrsFlow.Handlers;
using BeComfy.Common.RabbitMq;
using BeComfy.Services.Tickets.Domain;
using BeComfy.Services.Tickets.Messages.Commands;
using BeComfy.Services.Tickets.Messages.Events;
using BeComfy.Services.Tickets.Repositories;

namespace BeComfy.Services.Tickets.CommandHandlers
{
    public class BuyTicketHandler : ICommandHandler<BuyTicket>
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IBusPublisher _busPublisher;

        public BuyTicketHandler(ITicketsRepository ticketsRepository, IBusPublisher busPublisher)
        {
            _ticketsRepository = ticketsRepository;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(BuyTicket command, ICorrelationContext context)
        {
            // TODO : Customer validation -> call to Customers microservice (or personal CustomerId db) - for now Customer is always valid

            // TODO : Calculate ticket price -> call to Pricing microservice - hardcode for now
            // TODO : Add discounts -> call to Discounts microservice
            var totalTicketPrice = 250M;
            
            // TODO : Validate customer wallet -> call to Customers microservice - for now customer always have enough money

            var ticket = new Ticket(command.Id, command.FlightId, command.CustomerId, 
                totalTicketPrice, command.Seats);

            await _ticketsRepository.AddAsync(ticket);
            await _busPublisher.PublishAsync(new TicketBought(ticket.Id, ticket.Owner, ticket.Seats,
                    totalTicketPrice), context);
        }
    }
}