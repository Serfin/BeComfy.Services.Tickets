using System;
using BeComfy.Common.Types.Enums;

namespace BeComfy.Services.Tickets.Domain
{
    // TODO : Add domain validation
    public class Ticket
    {
        public Guid Id { get; }
        public Guid FlightId { get; private set; }
        public Guid Owner { get; private set; }
        public SeatClass SeatClass { get; private set; }

        public Ticket(Guid id, Guid flightId, Guid owner, SeatClass seatClass)
        {
            Id = id;
            SetFlightId(flightId);
            SetOwner(owner);
            SetSeatClass(seatClass);
        }

        private void SetFlightId(Guid flightId)
        {
            FlightId = flightId;
        }

        private void SetOwner(Guid owner)
        {
            Owner = owner;
        }

        private void SetSeatClass(SeatClass seatClass)
        {
            SeatClass = seatClass;
        }
    }
}