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
        public ApplicationDbContext() { }
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
            //The employees' positions are specified directly in the migration as to avoid complexity
            PasswordHasher<EmployeeApplicationUser> hasher = new PasswordHasher<EmployeeApplicationUser>();
            modelBuilder.Entity<EmployeeApplicationUser>().HasData(
                new EmployeeApplicationUser { Id = 1, UserName = "Admin", PasswordHash = hasher.HashPassword(null, "Admin1/") },
                new EmployeeApplicationUser { Id = 2, UserName = "NoEmployee" }
                );
            modelBuilder.Entity<PositionApplicationRole>().HasData(
                new PositionApplicationRole() { Id = 1, Name = "Admin" },
                new PositionApplicationRole() { Id = 2, Name = "No Position!" }
                );
            modelBuilder.Entity<Guest>().HasData(
                new Guest() { ID = 1, Name = "No Guest!" }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
