using API.Middleware;
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
        [EndpointSummary("Create a device")]
        [ProducesResponseType<DeviceResponse>(StatusCodes.Status201Created, "application/json", Description = "Creates and returns the device")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json", Description = "Invalid create request")]
        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> Create(CreateDeviceRequest request)
        {
            var device = await _deviceService.CreateAsync(request);

            return CreatedAtAction(nameof(Get),
                new { id = device.Id }, device);
        }

        [EndpointSummary("Get a device")]
        [ProducesResponseType<DeviceResponse>(StatusCodes.Status200OK, "application/json", Description = "Returns the device")]
        [ProducesResponseType<BaseErrorApiResponse>(StatusCodes.Status404NotFound, "application/json", Description = "Device not found")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DeviceResponse>> GetById([FromRoute] Guid id)
        {
            return Ok(await _deviceService.GetByIdAsync(id));
        }

        [EndpointSummary("List devices")]
        [EndpointDescription("List devices, optionally filtering by brand and/or state")]
        [ProducesResponseType<List<DeviceResponse>>(StatusCodes.Status200OK, "application/json", Description = "Returns the device list")]
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

        [EndpointSummary("Update a device")]
        [EndpointDescription("Fully or partially update device properties")]
        [ProducesResponseType<List<DeviceResponse>>(StatusCodes.Status200OK, "application/json", Description = "Updates the device")]
        [ProducesResponseType<BaseErrorApiResponse>(StatusCodes.Status400BadRequest, "application/json", Description = "Device cannot be updated")]
        [ProducesResponseType<BaseErrorApiResponse>(StatusCodes.Status404NotFound, "application/json", Description = "Device not found")]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<DeviceResponse>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateDeviceRequest request)
        {
            var device = await _deviceService.UpdateAsync(id, request);
            return Ok(device);
        }

        [EndpointSummary("Delete a device")]
        [EndpointDescription("Delete a device. Fails if the device is in use")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Deletes the device")]
        [ProducesResponseType<BaseErrorApiResponse>(StatusCodes.Status400BadRequest, "application/json", Description = "Device cannot be deleted")]
        [ProducesResponseType<BaseErrorApiResponse>(StatusCodes.Status404NotFound, "application/json", Description = "Device not found")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _deviceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
