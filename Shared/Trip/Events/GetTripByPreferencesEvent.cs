namespace Shared.Trip.Events
{
    public class GetTripByPreferencesEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureDate  { get; set; }
        public string DeparturePlace { get; set; } = string.Empty;
        public int NumOfAdults { get; set; }
        public int NumOfKidsTo18 { get; set; }
        public int NumOfKidsTo10 { get; set; }
        public int NumOfKidsTo3 { get; set; }
    }
}