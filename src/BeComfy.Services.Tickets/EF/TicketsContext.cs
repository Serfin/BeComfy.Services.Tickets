using System.Collections.Generic;
using BeComfy.Common.Types.Enums;
using BeComfy.Services.Tickets.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BeComfy.Services.Tickets.EF
{
    public class TicketsContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }

        public TicketsContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Ticket>()
                .Property(x => x.CreatedAt)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Ticket>()
                .Property(x => x.TotalCost)
                .HasColumnType("money");
            
            modelBuilder.Entity<Ticket>()
                .Property(x => x.Seats)
                .HasConversion(
                    @in => JsonConvert.SerializeObject(@in), 
                    @out => JsonConvert.DeserializeObject<IDictionary<SeatClass, int>>(@out));

        }
    }
}