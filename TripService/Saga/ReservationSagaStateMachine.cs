using MassTransit;
using Shared.Flight.Events;
using Shared.Trip.Events;

namespace TripService.Saga
{
    public class ReservationStateMachine : MassTransitStateMachine<ReservationState>
    {
        public State TempReserved { get; set; }
        public State AwaitingForHotelReservation { get; set; }
        public State AwaitingForTransportAndHotelReservation { get; set; }
        public State HotelAndTransportSuccessfullyReserved { get; set; }
        public State AwaitingForPayment { get; set; }
        public State TripReservedCorrectly { get; set; }
        public State ReservationFailed { get; set; }

        public Event<ReservationTripEvent> ReservationTripEvent {get; set;}
        public Event<ReserveSeatsEvent> ReserveSeatsEvent {get; set;}



        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState, TempReserved, AwaitingForHotelReservation, AwaitingForTransportAndHotelReservation, HotelAndTransportSuccessfullyReserved, AwaitingForPayment, TripReservedCorrectly, ReservationFailed);
        }
    }
}