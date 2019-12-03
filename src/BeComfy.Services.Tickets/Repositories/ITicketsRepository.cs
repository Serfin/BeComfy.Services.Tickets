using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeComfy.Services.Tickets.Domain;

namespace BeComfy.Services.Tickets.Repositories
{
    public interface ITicketsRepository
    {
        Task AddAsync(Ticket ticker);
        Task<Ticket> GetAsync(Guid id);
        Task<IEnumerable<Ticket>> BrowseAsync(int pageSize, int page);
        Task UpdateAsync(Ticket ticket);
        Task DeleteTAsync(Guid id);
    }
}