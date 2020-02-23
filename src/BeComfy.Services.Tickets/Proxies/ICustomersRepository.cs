using System;
using System.Threading.Tasks;
using BeComfy.Services.Tickets.Services.ServiceModels;
using RestEase;

namespace BeComfy.Services.Tickets.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface ICustomersService
    {
        [AllowAnyStatusCode]
        [Get("customers/{id}")]
        Task<Customer> GetAsync([Path] Guid id);
    }
}