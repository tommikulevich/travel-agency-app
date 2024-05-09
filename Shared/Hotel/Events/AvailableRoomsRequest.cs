namespace Shared.Hotel.Events {
    public class AvailableRoomsRequest {
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}