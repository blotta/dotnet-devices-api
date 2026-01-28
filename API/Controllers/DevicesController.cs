using Application.DTOs;
using Application.Services;
using Domain.Enums;
using Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController(DeviceService _deviceService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> Create(CreateDeviceRequest request)
        {
            var device = await _deviceService.CreateAsync(request);

            return CreatedAtAction(nameof(Get),
                new { id = device.Id }, device);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DeviceResponse>> GetById([FromRoute] Guid id)
        {
            return Ok(await _deviceService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<List<DeviceResponse>>> Get(
            [FromQuery] string? brand,
            [FromQuery] DeviceState? state
        )
        {
            var filter = new DeviceFilter(brand, state);
            var list = await _deviceService.GetAsync(filter);

            return Ok(list);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateDeviceRequest request)
        {
            var device = await _deviceService.UpdateAsync(id, request);
            return Ok(device);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _deviceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
