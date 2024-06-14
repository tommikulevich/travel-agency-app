using Shared.Trip.Dtos;
using TripService.Models;

namespace TripService.Data
{
    public class TripRepo : ITripRepo
    {
        private readonly AppDbContext _context;

        public TripRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateTrip(Trip offer)
        {
            if (offer == null) 
            {
                throw new ArgumentNullException(nameof(offer));
            }
            _context.Trip.Add(offer);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trip.ToList();
        }

        public IEnumerable<Trip> GetTripsByPreferences(string Destination, DateTime DepartureDate, 
                string DeparturePlace, int NumOfAdults, int NumOfKidsTo18, int NumOfKidsTo10, int NumOfKidsTo3)
        {
            DateTime utcDepartureDate = DepartureDate.ToUniversalTime();

            IQueryable<Trip> query = _context.Trip;
            query = query.Where(t => t.Country == Destination);
            query = query.Where(t => t.DepartureDate.Date == utcDepartureDate.Date);
            query = query.Where(t => t.DeparturePlace == DeparturePlace);
            query = query.Where(t => t.NumOfAdults == NumOfAdults);
            query = query.Where(t => t.NumOfKidsTo18 == NumOfKidsTo18);
            query = query.Where(t => t.NumOfKidsTo10 == NumOfKidsTo10);
            query = query.Where(t => t.NumOfKidsTo3 == NumOfKidsTo3);

            return query.ToList();
        }

        public List<Trip> GetTripById(string id)
        {
            Guid guid = Guid.Parse(id);
            return _context.Trip.Where(p => p.ClientId == guid).ToList();
        }

        public Trip GetTripByGuid(Guid guid)
        {
            var trip = _context.Trip.FirstOrDefault(t => t.Id == guid);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {guid} not found.");
            }

            return trip;
        }

        public void SaveTrip(Trip Trip)
        {
            _context.Trip.Add(Trip);
            _context.SaveChanges();
        }

        public bool CheckAvailability(Guid TripId)
        {
            var trip = _context.Trip.FirstOrDefault(p => p.Id == TripId);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {TripId} not found.");
            }

            bool result;
            string status = trip.Status;
            if (status == "Dostępna")
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public void ChangeReservationStatus(Guid TripId, string newReservationStatus, Guid? UserId)
        {
            var trip = _context.Trip.FirstOrDefault(t => t.Id == TripId);
            if (trip != null)
            {
                trip.ClientId = UserId;
                trip.Status = newReservationStatus;
            }
            else
            {
                Console.WriteLine($"Trip with ID {TripId} not found.");
            }
            _context.SaveChanges();
        }

        public string GetMostPopularReservedDestination()
        {
            var reservedOffers = _context.Trip
                .Where(o => o.Status == "Zarezerwowana")
                .ToList();

            var destinationCount = reservedOffers
                .GroupBy(o => o.Country)
                .Select(g => new
                {
                    Destination = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var mostPopularDestination = destinationCount.FirstOrDefault();

            if(mostPopularDestination != null) 
            {
                 return mostPopularDestination.Destination;
            }

            return "";
        }

        public string GetMostPopularReservedHotel()
        {
            var reservedOffers = _context.Trip
                .Where(o => o.Status == "Zarezerwowana")
                .ToList();

            var hotelCount = reservedOffers
                .GroupBy(o => o.Name)
                .Select(g => new
                {
                    Destination = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var mostPopularHotel = hotelCount.FirstOrDefault();

            if(mostPopularHotel != null) 
            {
                 return mostPopularHotel.Destination;
            }

            return "";
        }

        public string GetMostPopularReservedRoom()
        {
            var reservedOffers = _context.Trip
                .Where(o => o.Status == "Zarezerwowana")
                .ToList();

            var roomCount = reservedOffers
                .GroupBy(o => o.RoomType)
                .Select(g => new
                {
                    Destination = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var mostPopularRoom = roomCount.FirstOrDefault();

            if(mostPopularRoom != null) 
            {
                 return mostPopularRoom.Destination;
            }

            return "";
        }

        public string GetMostPopularReservedTransport()
        {
            var reservedOffers = _context.Trip
                .Where(o => o.Status == "Zarezerwowana")
                .ToList();

            var transportCount = reservedOffers
                .GroupBy(o => o.TransportType)
                .Select(g => new
                {
                    Destination = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var mostPopularTransport = transportCount.FirstOrDefault();
            if(mostPopularTransport != null) 
            {
                 return mostPopularTransport.Destination;
            }

            return "";
        }

        public IEnumerable<Trip> GetTripsBySpecificRoomConfiguration(Guid HotelId, int NumOfAdults, 
                int NumOfKidsTo18, int NumOfKidsTo10, int NumOfKidsTo3, DateTime ArrivalDate,
                DateTime ReturnDate, string RoomType)
        {
            IQueryable<Trip> query = _context.Trip.Where(t => 
                t.HotelId == HotelId 
                && t.NumOfAdults == NumOfAdults
                && t.NumOfKidsTo18 == NumOfKidsTo18
                && t.NumOfKidsTo10 == NumOfKidsTo10
                && t.NumOfKidsTo3 == NumOfKidsTo3
                && t.DepartureDate == ArrivalDate.ToUniversalTime()
                && t.ReturnDate == ReturnDate.ToUniversalTime()
                && t.RoomType == RoomType
            );

            return query.ToList();
        }

        public IEnumerable<Trip> GetTripsByFlightId(Guid? flightId)
        {
            IQueryable<Trip> query = _context.Trip.Where(p => 
                p.FlightId == flightId
            );
            
            return query.ToList();
        }

        public void CreateChangesEvent(ChangesEvent change)
        {
            if (change == null) 
            {
                throw new ArgumentNullException(nameof(change));
            }
            _context.ChangesEvent.Add(change);
        }

        public ChangesEventDto GetRandomTripAndGenerateChanges()
        {
            Random rand = new Random();
            List<Trip> specificTrips;
            string changeType = rand.Next(10) < 6 ? "Price" : "Status";
            if (changeType == "Price")
            {
                specificTrips = _context.Trip.Where(
                t => t.Status == "Dostępna").ToList();
            }
            else
            {
                specificTrips = _context.Trip.Where(
                t => t.Status == "Dostępna" 
                  || t.Status == "Zarezerwowana" 
                  || t.Status == "Odwołana").ToList();
            }
            
            if (specificTrips.Count == 0)
            {
                Console.WriteLine("There is no offers with statuses: Dostępna, Zarezerwowana, Odwołana!");
                return new ChangesEventDto();
            }

            
            int index = rand.Next(specificTrips.Count);
            var selectedTrip = specificTrips[index];
            Console.WriteLine($"Generator selects offer: {selectedTrip.Id}");

            // Decide if we are going to change price or status (price has 60% probability)
            string changeValue = "-";
            string previousValue = "-";

            if (changeType == "Price")
            {
                double oldPrice = selectedTrip.Price;
                double maxIncrease = oldPrice;
                double newPrice = oldPrice + (int)(rand.NextDouble() * maxIncrease);

                Console.WriteLine($"Old price: {oldPrice}. Generated price: {newPrice}");
                selectedTrip.Price = newPrice;
                changeValue = newPrice.ToString();
                previousValue = oldPrice.ToString();
            }
            else
            {
                string newStatus = "-";
                string oldStatus = selectedTrip.Status;
                Guid? clientId = selectedTrip.ClientId;
                switch (oldStatus)
                {
                    case "Dostępna":
                        newStatus = "Odwołana";
                        break;
                    case "Odwołana":
                        newStatus = "Dostępna";
                        break;
                    case "Zarezerwowana":
                        // TODO: unreserve hotel and flight
                        newStatus = "Dostępna";
                        clientId = null;

                        // TODO: (optional) add possible changing to "Odwołana"
                        break;
                }
                Console.WriteLine($"Old status: {oldStatus}. Generated status: {newStatus}");
                changeValue = newStatus;
                previousValue = oldStatus;
            }

            // Update changes
            ChangesEvent changes = new ChangesEvent{
                Id = Guid.NewGuid(),
                OfferId = selectedTrip.Id,
                ChangeType = changeType,
                ChangeValue = changeValue,
            };
            // CreateChangesEvent(changes);

            ChangesEventDto dto = new ChangesEventDto{
                CorrelationId = Guid.NewGuid(),
                OfferId = selectedTrip.Id,
                ChangeType = changeType,
                ChangeValue = changeValue,
                PreviousValue = previousValue
            };

            _context.SaveChanges();

            return dto;
        }
    }
}