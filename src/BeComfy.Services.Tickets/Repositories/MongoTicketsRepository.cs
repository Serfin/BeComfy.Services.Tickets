using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BeComfy.Common.Mongo;
using BeComfy.Services.Tickets.Domain;

namespace BeComfy.Services.Tickets.Repositories
{
    public class MongoTicketsRepository : ITicketsRepository
    {
        private readonly IMongoRepository<Ticket> _collection;

        public MongoTicketsRepository(IMongoRepository<Ticket> collection)
        {
            _collection = collection;
        }

        public async Task AddAsync(Ticket entity)
            => await _collection.AddAsync(entity);

        public async Task<IEnumerable<Ticket>> BrowseAsync(int pageSize, int page)
            => await _collection.BrowseAsync(pageSize, page);

        public async Task<IEnumerable<Ticket>> BrowseAsync(int pageSize, int page, Expression<Func<Ticket, bool>> predicate)
            => await _collection.BrowseAsync(pageSize, page, predicate);

        public async Task DeleteAsync(Guid id)
            => await _collection.DeleteAsync(id);

        public async Task<Ticket> GetAsync(Guid id)
            => await _collection.GetAsync(x => x.Id == id);

        public async Task UpdateAsync(Ticket entity)
            => await _collection.UpdateAsync(entity);
    }
}