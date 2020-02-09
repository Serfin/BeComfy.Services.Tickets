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
        private readonly ICustomersService _customersService;

        private const decimal PriceForEconomic = 255M;
        private const decimal PriceForBusiness = 675M;

        public BuyTicketHandler(ITicketsRepository ticketsRepository, IBusPublisher busPublisher,
            IFlightsService flightsService, ICustomersService customersService)
        {
            _ticketsRepository = ticketsRepository;
            _busPublisher = busPublisher;
            _flightsService = flightsService;
            _customersService = customersService;
        }

        public async Task HandleAsync(BuyTicket command, ICorrelationContext context)
        {
            var customer = await _customersService.GetAsync(command.CustomerId);
            if (customer is null)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Customer with id: {command.CustomerId} does not exist");
            }
            
            // TODO : Add discounts -> call to Discounts microservice

            var flight = await _flightsService.GetAsync(command.FlightId);
            if (flight is null)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Flight with id: {command.FlightId} does not exist");
            }

            if (flight.FlightStatus != FlightStatus.Continues)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Flight has status: {flight.FlightStatus} which is invalid to buy ticket");
            }
            
            // TODO : Calculate ticket price -> call to Pricing microservice - hardcode for now
            var totalTicketPrice = 0M;
            foreach (var (seatClass, count) in command.Seats)
            {
                switch (seatClass) {
                    case SeatClass.Economic:
                        totalTicketPrice += count * PriceForEconomic;
                        break;
                    
                    case SeatClass.Business:
                        totalTicketPrice += count * PriceForBusiness;
                        break;
                    
                    default:
                        throw new BeComfyException("cannot_buy_ticket", "Invalid seat class");
                }
            }

            if (customer.Balance - totalTicketPrice < 0)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Customer with id: {customer.Id} does not have enough money to buy ticket");
            }
            
            var ticket = new Ticket(command.Id, command.FlightId, customer.Id, 
                totalTicketPrice, command.Seats);

            await _ticketsRepository.AddAsync(ticket);
            await _busPublisher.PublishAsync(new TicketBought(ticket.Id, ticket.Owner, ticket.TotalCost), context);
        }
    }
}