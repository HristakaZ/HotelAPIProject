using DataStructure;
using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ReservationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Reservation> GetReservations()
        {
            return dbContext.Reservations.ToList();
        }

        public Reservation GetReservationByID(int id)
        {
            return dbContext.Reservations.Where(x => x.ID == id).FirstOrDefault();
        }

        public void CreateReservation(Reservation reservation)
        {
            dbContext.Reservations.Add(reservation);
        }

        public void UpdateReservation(int id)
        {
            Reservation reservation = GetReservationByID(id);
            dbContext.Reservations.Update(reservation);
        }

        public void DeleteReservation(int id)
        {
            Reservation reservation = GetReservationByID(id);
            dbContext.Reservations.Remove(reservation);
        }

    }
}
