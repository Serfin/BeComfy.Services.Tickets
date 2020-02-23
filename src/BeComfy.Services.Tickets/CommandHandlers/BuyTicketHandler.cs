using System.Threading.Tasks;
using BeComfy.Common.CqrsFlow.Handlers;
using BeComfy.Common.RabbitMq;
using BeComfy.Common.Types.Enums;
using BeComfy.Common.Types.Exceptions;
using BeComfy.Services.Tickets.Domain;
using BeComfy.Services.Tickets.Messages.Commands;
using BeComfy.Services.Tickets.Services.Interfaces;

namespace BeComfy.Services.Tickets.CommandHandlers
{
    public class BuyTicketHandler : ICommandHandler<BuyTicket>
    {
        private readonly ITicketsService _ticketsService;
        public BuyTicketHandler(ITicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }

        public async Task HandleAsync(BuyTicket command, ICorrelationContext context)
            => await _ticketsService.BuyTicket(command, context);
    }
}