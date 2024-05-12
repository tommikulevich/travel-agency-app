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
        readonly IRequestClient<ReservationTripEvent> _ReservationTripEvent;

        public TripController(IRequestClient<GetTripsByUserIdEvent> getTripsClient, 
                            IRequestClient<GetAllTripsEvent> getAllTrips, 
                            IRequestClient<GetTripByPreferencesEvent> getTripsByPreferences,
                            IRequestClient<ReservationTripEvent> ReservationTripEvent)
        {
            _getTripsClient = getTripsClient;
            _getAllTrips = getAllTrips;
            _GetTripsByPreferences = getTripsByPreferences;
            _ReservationTripEvent = ReservationTripEvent;
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
        [HttpPost("ReserveTrip")]
        public async Task<ReservationTripReplyEvent> ReserveTrip(ReservationDto reservationDto)
        {
            var reservationId = Guid.NewGuid();
            var request = new ReservationTripEvent()
            {
                Id = reservationId,
                CorrelationId = reservationId,
                ClientId = reservationDto.ClientId,
                FlightId = reservationDto.FlightId,
                HotelId = reservationDto.HotelId,
                Name = reservationDto.Name,
                Country = reservationDto.Country,
                DeparturePlace = reservationDto.DeparturePlace,
                ArrivalPlace = reservationDto.ArrivalPlace,
                NumOfAdults = reservationDto.NumOfAdults,
                NumOfKidsTo18 = reservationDto.NumOfKidsTo18,
                NumOfKidsTo10 = reservationDto.NumOfKidsTo10,
                NumOfKidsTo3 = reservationDto.NumOfKidsTo3,
                DepartureDate = reservationDto.DepartureDate,
                ReturnDate = reservationDto.ReturnDate,
                TransportType = reservationDto.TransportType,
                Price = reservationDto.Price,
                MealsType = reservationDto.MealsType,
                RoomType = reservationDto.RoomType,
                DiscountPercents = reservationDto.DiscountPercents,
                NumOfNights = reservationDto.NumOfNights
            };
            var response = await _ReservationTripEvent.GetResponse<ReservationTripReplyEvent>(request);
            return response.Message;
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