using Core.Device.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Core.Device.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        readonly IDeviceRepository _repository;

        public DeviceController(IDeviceRepository repository)
        {
            _repository = repository;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string key)
        {
            var devices = await _repository.ListAsync(p => p.Id.Equals(key));
            if (!devices.Any()) return NoContent();

            return Ok(devices);
        }

        [HttpGet("pushToken")]
        public async Task<IActionResult> GetPushToken([FromQuery] int customerId, CancellationToken cancellationToken)
        {
            var deviceData = await _repository.GetPushTokenAsync(customerId, cancellationToken);

            if (string.IsNullOrEmpty(deviceData)) return NotFound();

            return Ok(deviceData);
        }
    }
}
