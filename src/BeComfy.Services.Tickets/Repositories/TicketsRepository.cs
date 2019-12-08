using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeComfy.Common.Types.Exceptions;
using BeComfy.Services.Tickets.Domain;
using BeComfy.Services.Tickets.EF;
using Microsoft.EntityFrameworkCore;

namespace BeComfy.Services.Tickets.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly TicketsContext _context;

        public TicketsRepository(TicketsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ticket>> BrowseAsync(int pageSize, int page = 1)
            => await _context.Tickets.OrderBy(x => x.CreatedAt)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        public async Task DeleteAsync(Guid ticketId)
        {
            var ticket = await GetAsync(ticketId);

            if (ticket is null)
            {
                throw new BeComfyException($"Ticket with id: {ticketId} does not exist.");
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket> GetAsync(Guid id)
            => await _context.Tickets.FindAsync(id);
    }
}