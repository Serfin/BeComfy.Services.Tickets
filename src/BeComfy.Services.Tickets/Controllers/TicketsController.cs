using System.Threading.Tasks;
using BeComfy.Common.CqrsFlow.Dispatcher;
using BeComfy.Services.Tickets.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BeComfy.Services.Tickets.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketsController : BaseController
    {
        public TicketsController(IQueryDispatcher queryDispatcher)
            : base(queryDispatcher)
        {

        }
        
        [HttpGet]
        public async Task<IActionResult> GetTicketsForCustomer([FromQuery] GetTicketsForCustomer query)
            => Ok(await QueryAsync(query));
    }
}