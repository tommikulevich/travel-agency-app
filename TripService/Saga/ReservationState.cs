using MassTransit;

namespace TripService.Saga
{
    public class ReservationState : SagaStateMachineInstance
    {
        public int CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
    }
}