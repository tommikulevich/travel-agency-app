namespace Shared.Trip.Events
{
    public class GetTripByPreferencesEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate  { get; set; }
        public string DeparturePlace { get; set; }
        public int NumOfAdults { get; set; }
        public int NumOfKidsTo18 { get; set; }
        public int NumOfKidsTo10 { get; set; }
        public int NumOfKidsTo3 { get; set; }

    }

}