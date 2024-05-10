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

        readonly IRequestClient<GetTripByPreferencesEvent> _GetTripsByPreferences;

        public TripController(IRequestClient<GetTripsByUserIdEvent> getTripsClient, 
                            IRequestClient<GetAllTripsEvent> getAllTrips, 
                            IRequestClient<GetTripByPreferencesEvent> getTripsByPreferences)
        {
            _getTripsClient = getTripsClient;
            _getAllTrips = getAllTrips;
            _GetTripsByPreferences = getTripsByPreferences;
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

        [HttpGet("GetTripsByPreferences")]
        public async Task<IEnumerable<TripDto>> GetTripsByPreferences(string Destination, DateTime DepartureDate, 
                                                                string DeparturePlace, int NumOfAdults, 
                                                                int NumOfKidsTo18, int NumOfKidsTo10, 
                                                                int NumOfKidsTo3)
        {
            // var userGuid = Guid.Parse(userId);
            Console.WriteLine("-> Getting trips by preferences...");
            var queryEvent = new GetTripByPreferencesEvent
            {
                CorrelationId = Guid.NewGuid(),
                Destination = Destination,
                DepartureDate = DepartureDate,
                DeparturePlace = DeparturePlace,
                NumOfAdults = NumOfAdults,
                NumOfKidsTo18 = NumOfKidsTo18,
                NumOfKidsTo10 = NumOfKidsTo10,
                NumOfKidsTo3 = NumOfKidsTo3
            };
            var response = await _GetTripsByPreferences.GetResponse<ReplyTripsDtosEvent>(queryEvent);
            var Trips = response.Message.Trips;
            return Trips;
        }
    }
}