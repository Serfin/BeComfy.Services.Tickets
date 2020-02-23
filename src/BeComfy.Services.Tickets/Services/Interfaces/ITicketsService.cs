using System.Threading.Tasks;
using BeComfy.Common.RabbitMq;
using BeComfy.Services.Tickets.Messages.Commands;

namespace BeComfy.Services.Tickets.Services.Interfaces
{
    public interface ITicketsService
    {
        Task BuyTicket(BuyTicket command, ICorrelationContext context);
    }
}