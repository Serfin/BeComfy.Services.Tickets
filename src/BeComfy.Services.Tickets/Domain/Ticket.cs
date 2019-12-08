using System;
using System.Collections.Generic;
using System.Linq;
using BeComfy.Common.Types.Enums;
using BeComfy.Common.Types.Exceptions;

namespace BeComfy.Services.Tickets.Domain
{
    // TODO : Add domain validation
    public class Ticket
    {
        public Guid Id { get; }
        public Guid FlightId { get; private set; }
        public Guid Owner { get; private set; }
        public IDictionary<SeatClass, int> Seats { get; private set; }
        public decimal TotalCost { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }

        public Ticket()
        {
            
        }

        public Ticket(Guid id, Guid flightId, Guid owner, decimal totalCost, IDictionary<SeatClass, int> seats)
        {
            Id = id;
            SetFlightId(flightId);
            SetOwner(owner);
            SetTotalCost(totalCost);
            SetSeatClass(seats);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.MinValue;
        }

        private void SetFlightId(Guid flightId)
        {
            FlightId = flightId;
        }

        private void SetOwner(Guid owner)
        {
            Owner = owner;
        }

        private void SetTotalCost(decimal totalCost)
        {
            if (totalCost <= 0)
            {
                throw new BeComfyDomainException($"{nameof(totalCost)} cannot be less or equal to 0");
            }
            
            TotalCost = totalCost;
        }

        private void SetSeatClass(IDictionary<SeatClass, int> seats)
        {
            if (seats is null)
            {
                throw new BeComfyDomainException($"{nameof(seats)} cannot be null");
            }

            var seatCount = seats.Sum(seat => seat.Value);

            if (seatCount <= 0)
            {
                throw new BeComfyDomainException("Total count of seats cannot be less or equal to 0");
            }

            Seats = seats;
        }

        private void SetUpdateDate()
            => UpdatedAt = DateTime.UtcNow;
    }
}