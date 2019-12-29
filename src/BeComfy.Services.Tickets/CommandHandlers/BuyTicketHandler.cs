using System.Threading.Tasks;
using BeComfy.Common.CqrsFlow.Handlers;
using BeComfy.Common.RabbitMq;
using BeComfy.Common.Types.Enums;
using BeComfy.Common.Types.Exceptions;
using BeComfy.Services.Tickets.Domain;
using BeComfy.Services.Tickets.Messages.Commands;
using BeComfy.Services.Tickets.Messages.Events;
using BeComfy.Services.Tickets.Repositories;
using BeComfy.Services.Tickets.Services;

namespace BeComfy.Services.Tickets.CommandHandlers
{
    public class BuyTicketHandler : ICommandHandler<BuyTicket>
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly IFlightsService _flightsService;

        private const decimal PriceForEconomic = 255M;
        private const decimal PriceForBusiness = 675M;

        public BuyTicketHandler(ITicketsRepository ticketsRepository, IBusPublisher busPublisher,
            IFlightsService flightsService)
        {
            _ticketsRepository = ticketsRepository;
            _busPublisher = busPublisher;
            _flightsService = flightsService;
        }

        public async Task HandleAsync(BuyTicket command, ICorrelationContext context)
        {
            // TODO : Customer validation -> call to Customers microservice (or personal CustomerId db) - for now Customer is always valid
            // TODO : Add discounts -> call to Discounts microservice

            var flight = await _flightsService.GetAsync(command.FlightId);
            if (flight is null)
            {
                throw new BeComfyException($"Flight with id: {command.FlightId} does not exist");
            }

            if (flight.FlightStatus != FlightStatus.Continues)
            {
                throw new BeComfyException($"Flight has status: {flight.FlightStatus} which is invalid to buy ticket");
            }
            
            // TODO : Calculate ticket price -> call to Pricing microservice - hardcode for now
            var totalTicketPrice = 0M;
            foreach (var (seatClass, count) in command.Seats)
            {
                if (seatClass is SeatClass.Economic)
                {
                    totalTicketPrice += count * PriceForEconomic;
                }
                else
                {
                    totalTicketPrice += count * PriceForBusiness;
                }
            }
            
            // TODO : Validate customer wallet -> call to Customers microservice - for now customer always have enough money, well, right now he doesn't even have wallet 
            var ticket = new Ticket(command.Id, command.FlightId, command.CustomerId, totalTicketPrice, command.Seats);

            await _ticketsRepository.AddAsync(ticket);
            await _busPublisher.PublishAsync(new TicketBought(ticket.Id, ticket.Owner, ticket.Seats,
                    totalTicketPrice), context);
        }
    }
}