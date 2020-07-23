using System;
using System.Collections.Generic;
using System.Text;
using DataStructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel_API_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomReservation>()
                .HasKey(x => new { x.RoomID, x.ReservationID });
            modelBuilder.Entity<RoomReservation>()
                .HasOne(x => x.Room)
                .WithMany(x => x.RoomReservations)
                .HasForeignKey(x => x.RoomID);
            modelBuilder.Entity<RoomReservation>()
                .HasOne(x => x.Reservation)
                .WithMany(x => x.RoomReservations)
                .HasForeignKey(x => x.ReservationID);
        }
    }
}
