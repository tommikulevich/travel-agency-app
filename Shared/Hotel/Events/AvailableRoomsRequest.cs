namespace Shared.Hotel.Events {
    public class AvailableRoomsRequest {
        public string DeparturePlace { get; set; } = string.Empty;
        public string ArrivalPlace { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}