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
        readonly IRequestClient<GetTripByPreferencesEvent> _getTripsByPreferences;
        readonly IRequestClient<ReservationTripEvent> _reservationTripEvent;
        readonly IRequestClient<CheckReservationStatusEvent> _checkReservationStatusEvent;

        public TripController(IRequestClient<GetTripsByUserIdEvent> getTripsClient, 
                              IRequestClient<GetAllTripsEvent> getAllTrips, 
                              IRequestClient<GetTripByPreferencesEvent> getTripsByPreferences,
                              IRequestClient<ReservationTripEvent> reservationTripEvent,
                              IRequestClient<CheckReservationStatusEvent> checkReservationStatusEvent)
        {
            _getTripsClient = getTripsClient;
            _getAllTrips = getAllTrips;
            _getTripsByPreferences = getTripsByPreferences;
            _reservationTripEvent = reservationTripEvent;
            _checkReservationStatusEvent = checkReservationStatusEvent;
        }

        [HttpGet("GetTripsByUserId")]
        public async Task<IEnumerable<TripDto>> GetTrips(string clientId)
        {
            Console.WriteLine("-> Getting trips reserved by user...");
            var response = await _getTripsClient.GetResponse<ReplyTripsDtosEvent>(
                new GetTripsByUserIdEvent() 
                { 
                    CorrelationId = Guid.NewGuid(), 
                    ClientId = clientId 
                });

            var Trips = response.Message.Trips;

            return Trips;
        }

        [HttpPost("ReserveTrip")]
        public async Task<ReservationTripReplyEvent> ReserveTrip(ReservationDto reservationDto)
        {
            var request = new ReservationTripEvent()
            {
                Id = Guid.NewGuid(),
                CorrelationId = reservationDto.OfferId,
                OfferId = reservationDto.OfferId,
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

            var checkReservationResponse = await _checkReservationStatusEvent.GetResponse<CheckReservationStatusReplyEvent>(
                new CheckReservationStatusEvent() 
                {
                    CorrelationId = reservationDto.OfferId
                });

            bool isAvailable = checkReservationResponse.Message.available;
            if (isAvailable)
            {
                var response = await _reservationTripEvent.GetResponse<ReservationTripReplyEvent>(request);
                return response.Message;
            }
            else
            {
                Console.WriteLine("Somebody just reserved your offer");
                return new ReservationTripReplyEvent() {};
            }
        }
        
        [HttpGet("GetAllTrips")]
        public async Task<IEnumerable<TripDto>> GetAllTrips()
        {
            Console.WriteLine("-> Getting all trips...");
            var response = await _getAllTrips.GetResponse<ReplyTripsDtosEvent>(
                new GetAllTripsEvent() 
                { 
                    CorrelationId = Guid.NewGuid()
                });

            var Trips = response.Message.Trips;

            return Trips;
        }

        [HttpGet("GetTripsByPreferences")]
        public async Task<IEnumerable<TripDto>> GetTripsByPreferences(string destination, DateTime departureDate, 
                string departurePlace, int numOfAdults, int numOfKidsTo18, int numOfKidsTo10, int numOfKidsTo3)
        {
            Console.WriteLine("-> Getting trips by preferences...");
            var queryEvent = new GetTripByPreferencesEvent
            {
                CorrelationId = Guid.NewGuid(),
                Destination = destination,
                DepartureDate = departureDate,
                DeparturePlace = departurePlace,
                NumOfAdults = numOfAdults,
                NumOfKidsTo18 = numOfKidsTo18,
                NumOfKidsTo10 = numOfKidsTo10,
                NumOfKidsTo3 = numOfKidsTo3
            };
            var response = await _getTripsByPreferences.GetResponse<ReplyTripsDtosEvent>(queryEvent);

            var Trips = response.Message.Trips;

            return Trips;
        }
    }
}