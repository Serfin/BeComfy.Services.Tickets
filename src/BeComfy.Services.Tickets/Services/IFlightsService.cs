using System;
using System.Threading.Tasks;
using BeComfy.Services.Tickets.Services.ServiceModels;
using RestEase;

namespace BeComfy.Services.Tickets.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IFlightsService
    {
        [AllowAnyStatusCode]
        [Get("flights/{id}")]
        Task<Flight> GetAsync([Path] Guid id);
    }
}