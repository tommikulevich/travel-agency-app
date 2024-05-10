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
        readonly IRequestClient<GetTripsByUserIdEvent> _getTripsClient;

        readonly IRequestClient<GetAllTripsEvent> _getAllTrips;

        public TripController(IRequestClient<GetTripsByUserIdEvent> getTripsClient, IRequestClient<GetAllTripsEvent> getAllTrips)
        {
            _getTripsClient = getTripsClient;
            _getAllTrips = getAllTrips;
        }

        [HttpGet("GetTripsByUserId")]
        //[Route("GetTrips")]
        public async Task<IEnumerable<TripDto>> GetTrips(string ClientId)
        {
            // var userGuid = Guid.Parse(userId);
            Console.WriteLine("-> Getting trips reserved by user...");
            var response = await _getTripsClient.GetResponse<ReplyTripsDtosEvent>(new GetTripsByUserIdEvent() { CorrelationId = Guid.NewGuid(), ClientId = ClientId });
            var Trips = response.Message.Trips;
            return Trips;
        }
        
        [HttpGet("GetAllTrips")]
        public async Task<IEnumerable<TripDto>> GetAllTrips()
        {
            // var userGuid = Guid.Parse(userId);
            Console.WriteLine("-> Getting all trips...");
            var response = await _getAllTrips.GetResponse<ReplyTripsDtosEvent>(new GetAllTripsEvent() { CorrelationId = Guid.NewGuid()});
            var Trips = response.Message.Trips;
            return Trips;
        }
    }
}