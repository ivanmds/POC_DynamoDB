using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string account)
        {
            return Ok(await _eventRepository.ListEventsAsync(account));
        }
    }
}