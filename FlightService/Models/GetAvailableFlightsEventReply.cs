using System;
using System.Collections.Generic;
using FlightService.Data.Tables;

namespace FlightService.Events
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
