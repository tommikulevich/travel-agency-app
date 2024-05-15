using TripService.Models;

namespace TripService.Data
{
    public interface ITripRepo
    {
        // bool SaveChanges();

        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByPreferences(string Destination, DateTime DepartureDate, 
                                                                        string DeparturePlace, int NumOfAdults, 
                                                                        int NumOfKidsTo18, int NumOfKidsTo10, 
                                                                        int NumOfKidsTo3);
        List<Trip> GetTripById(string id);
        Trip GetTripByGuid(Guid guid);
        void CreateTrip(Trip Trip);
        bool CheckAvailability(Guid TripId);
        void SaveTrip(Trip Trip);
        public void ChangeReservationStatus(Guid TripId, string newReservationStatus, Guid? UserId);
        public IEnumerable<Trip> GetTripsBySpecificRoomConfiguration(Guid HotelId, int NumOfAdults, 
                int NumOfKidsTo18, int NumOfKidsTo10, int NumOfKidsTo3, DateTime ArrivalDate,
                DateTime ReturnDate, string RoomType);
        public IEnumerable<Trip> GetTripsByFlightId(Guid? flightId);
    }
}