using System;
using System.Collections.Generic;
using System.Text;
using DataStructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel_API_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<EmployeeApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeApplicationUser> Employees { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<PositionApplicationRole> Positions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
