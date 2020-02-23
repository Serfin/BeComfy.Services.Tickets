using System;

namespace BeComfy.Services.Tickets.Services.ServiceModels
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public decimal Balance { get; set; }
    }
}