using TripService.Models;

namespace TripService.Data
{
    public interface ITripRepo
    {
        bool SaveChanges();

        IEnumerable<Trip> GetAllTrips();
        List<Trip> GetTripById(string id);
        void CreateTrip(Trip Trip);
        void SaveTrip(Trip Trip);
    }
}