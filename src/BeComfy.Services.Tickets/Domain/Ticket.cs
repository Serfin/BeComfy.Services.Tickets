using System;
using System.Collections.Generic;
using System.Linq;
using BeComfy.Common.Mongo;
using BeComfy.Common.Types.Enums;
using BeComfy.Common.Types.Exceptions;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace BeComfy.Services.Tickets.Domain
{
    public class Ticket : IEntity
    {
        public Guid Id { get; }
        public Guid FlightId { get; private set; }
        public Guid Owner { get; private set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
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
            SetSeatClass(seats);
            SetTotalCost(totalCost);
            CreatedAt = DateTime.Now;
        }

        private void SetFlightId(Guid flightId)
        {
            if (flightId == null || flightId == Guid.Empty)
            {
                throw new BeComfyException("cannot_buy_ticket", $"{nameof(flightId)} cannot be null or empty");
            }

            FlightId = flightId;
            SetUpdateDate();
        }

        private void SetOwner(Guid owner)
        {
            if (owner == Guid.Empty)
            {
                throw new BeComfyException("cannot_buy_ticket", $"{nameof(owner)} cannot be empty");
            }

            Owner = owner;
            SetUpdateDate();
        }

        private void SetTotalCost(decimal totalCost)
        {
            if (totalCost <= 0)
            {
                throw new BeComfyException("cannot_buy_ticket", $"{nameof(totalCost)} cannot be less or equal to 0");
            }
            
            TotalCost = totalCost;
            SetUpdateDate();
        }

        private void SetSeatClass(IDictionary<SeatClass, int> seats)
        {
            if (seats is null)
            {
                throw new BeComfyException("cannot_buy_ticket", $"{nameof(seats)} cannot be null");
            }

            var seatCount = seats.Sum(seat => seat.Value);

            if (seatCount <= 0)
            {
                throw new BeComfyException("cannot_buy_ticket", "Total count of seats cannot be less or equal to 0");
            }

            Seats = seats;
            SetUpdateDate();
        }

        private void SetUpdateDate()
            => UpdatedAt = DateTime.UtcNow;
    }
}