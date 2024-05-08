using Microsoft.AspNetCore.Mvc;
using HotelService.Dtos;
using MassTransit;

namespace HotelService.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HotelsController : ControllerBase {
        private readonly IPublishEndpoint _publishEndpoint;

        public HotelsController(IPublishEndpoint publishEndpoint) {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("available")]
        public async Task<ActionResult> GetAvailableRooms([FromBody] AvailableRoomsRequest request) {
            await _publishEndpoint.Publish(request);
            return Ok("Request for available hotels has been sent.");
        }
    }
}