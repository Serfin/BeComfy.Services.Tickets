using System;
using System.Collections.Generic;
using BeComfy.Common.Types.Enums;

namespace BeComfy.Services.Tickets.Services.ServiceModels
{
    public class Flight
    {
        public Guid Id { get; set; }
        public IDictionary<SeatClass, int> AvailableSeats { get; set; }
        public FlightStatus FlightStatus { get; set; }
    }
}