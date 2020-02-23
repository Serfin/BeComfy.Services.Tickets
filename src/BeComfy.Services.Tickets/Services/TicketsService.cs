using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeComfy.Common.RabbitMq;
using BeComfy.Common.Types.Enums;
using BeComfy.Common.Types.Exceptions;
using BeComfy.Services.Tickets.Domain;
using BeComfy.Services.Tickets.Messages.Commands;
using BeComfy.Services.Tickets.Messages.Events;
using BeComfy.Services.Tickets.Repositories;
using BeComfy.Services.Tickets.Services.Interfaces;
using BeComfy.Services.Tickets.Services.ServiceModels;

namespace BeComfy.Services.Tickets.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly IFlightsService _flightsService;
        private readonly ICustomersService _customersService;

        private const decimal PriceForEconomic = 255M;
        private const decimal PriceForBusiness = 675M;

        public TicketsService(ITicketsRepository ticketsRepository, IBusPublisher busPublisher,
            IFlightsService flightsService, ICustomersService customersService)
        {
            _ticketsRepository = ticketsRepository;
            _busPublisher = busPublisher;
            _flightsService = flightsService;
            _customersService = customersService;
        }

        public async Task BuyTicket(BuyTicket command, ICorrelationContext context)
        {
            Customer customer = await LoadCustomer(command.CustomerId);
            if (customer is null)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Customer with id: {command.CustomerId} does not exist");
            }

            Flight flight = await LoadFlight(command.FlightId);
            if (flight is null)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Flight with id: {command.FlightId} does not exist");
            }

            if (flight.FlightStatus != FlightStatus.Continues)
            {
                throw new BeComfyException("cannot_buy_ticket", $"Flight has status: {flight.FlightStatus} which is invalid to buy ticket");
            }

            if (ValidateAvailableSeats(flight.AvailableSeats, command.Seats))
            {
                var totalTicketPrice = CalculateTicketTotalPrice(command.Seats);

                if (customer.Balance - totalTicketPrice < 0)
                {
                    throw new BeComfyException("cannot_buy_ticket", $"Customer with id: {customer.Id} does not have enough money to buy ticket");
                }
                
                var ticket = new Ticket(command.Id, command.FlightId, customer.Id, 
                    totalTicketPrice, command.Seats);

                await _ticketsRepository.AddAsync(ticket);
                await _busPublisher.PublishAsync(new TicketBought(ticket.Id, ticket.Owner, ticket.FlightId, ticket.TotalCost), context);        
            }
            else
            {
                throw new BeComfyException("cannot_buy_ticket", "Invalid seat count");             
            }
        }

        private decimal CalculateTicketTotalPrice(IDictionary<SeatClass, int> totalSeats)
        {
            decimal totalTicketPrice = 0M;
            foreach (var (seatClass, count) in totalSeats)
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

            return totalTicketPrice;
        }

        private bool ValidateAvailableSeats(IDictionary<SeatClass, int> flightAvailableSeats, IDictionary<SeatClass, int> ticketSeats)
            => flightAvailableSeats[SeatClass.Economic] >= ticketSeats[SeatClass.Economic] && 
                flightAvailableSeats[SeatClass.Business] >= ticketSeats[SeatClass.Business];

        private async Task<Customer> LoadCustomer(Guid id)
        {
            try 
            {
                return await _customersService.GetAsync(id);
            }
            catch (Exception)
            {
                // log connection exception to elastic
                throw;
            }
        }

        private async Task<Flight> LoadFlight(Guid id)
        {
            try 
            {
                return await _flightsService.GetAsync(id);
            }
            catch (Exception)
            {
                // log connection exception to elastic
                throw;
            }
        }
    }
}