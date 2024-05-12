using TripService.Models;

namespace TripService.Data
{
    public interface ITripRepo
    {
        bool SaveChanges();

        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByPreferences(string Destination, DateTime DepartureDate, 
                                                                        string DeparturePlace, int NumOfAdults, 
                                                                        int NumOfKidsTo18, int NumOfKidsTo10, 
                                                                        int NumOfKidsTo3);
        List<Trip> GetTripById(string id);
        void CreateTrip(Trip Trip);
        void SaveTrip(Trip Trip);
        public void ChangeReservationStatus(Guid TripId, string newReservationStatus);
    }
}