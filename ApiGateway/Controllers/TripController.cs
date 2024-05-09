using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Trip.Dtos;
using Shared.Trip.Events;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        readonly IRequestClient<GetTripEvent> _getTripsClient;

        public TripController(IRequestClient<GetTripEvent> getTripsClient)
        {
            _getTripsClient = getTripsClient;
        }

        [HttpGet("GetTrips")]
        //[Route("GetTrips")]
        public async Task<IEnumerable<TripDto>> GetTrips()
        {
            // var userGuid = Guid.Parse(userId);
            Console.WriteLine("Rest, jestem");
            var response = await _getTripsClient.GetResponse<GetTripReplyEvent>(new GetTripEvent() { CorrelationId = Guid.NewGuid(), UserId = "15" });
            var Trips = response.Message.Trips;
            return Trips;
        }
    }
}