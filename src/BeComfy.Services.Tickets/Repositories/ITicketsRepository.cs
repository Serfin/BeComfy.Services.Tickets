using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BeComfy.Services.Tickets.Domain;

namespace BeComfy.Services.Tickets.Repositories
{
    public interface ITicketsRepository
    {
        Task AddAsync(Ticket ticket);
        Task<Ticket> GetAsync(Guid id);
        Task<IEnumerable<Ticket>> BrowseAsync(int pageSize, int page);
        Task<IEnumerable<Ticket>> BrowseAsync(int pageSize, int page, 
            Expression<Func<Ticket, bool>> predicate);
        Task DeleteAsync(Guid id);
    }
}