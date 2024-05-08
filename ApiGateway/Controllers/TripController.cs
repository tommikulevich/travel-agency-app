using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ReservationService.Events;
using ReservationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        readonly IRequestClient<GetReservationEvent> _getReservationsClient;

        public ReservationController(IRequestClient<GetReservationEvent> getReservationsClient)
        {
            _getReservationsClient = getReservationsClient;
        }

        [HttpGet("GetReservations")]
        //[Route("GetReservations")]
        public async Task<IEnumerable<ReservationDto>> GetReservations()
        {
            // var userGuid = Guid.Parse(userId);
            Console.WriteLine("Rest, jestem");
            var response = await _getReservationsClient.GetResponse<GetReservationReplyEvent>(new GetReservationEvent() { CorrelationId = Guid.NewGuid(), UserId = "15" });
            var reservations = response.Message.Reservations;
            return reservations;
        }
    }
}