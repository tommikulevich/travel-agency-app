using System;
using System.Collections.Generic;
using FlightService.Data.Tables;

namespace Shared.Flight.Events
{
    public class GetAvailableFlightsEventReply
    {
        public List<FlightEntity> AvailableFlights { get; set; }

        public GetAvailableFlightsEventReply(List<FlightEntity> availableFlights)
        {
            AvailableFlights = availableFlights;
        }
    }
}
