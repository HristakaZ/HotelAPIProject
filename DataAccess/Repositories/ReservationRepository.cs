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

        public void UpdateReservation(Reservation reservation)
        {
            Reservation reservationToUpdate = GetReservationByID(reservation.ID);
            // this code might be extended later, for more property updates
            reservationToUpdate.StartDate = reservation.StartDate;
            dbContext.Reservations.Update(reservationToUpdate);
        }

        public void DeleteReservation(int id)
        {
            Reservation reservation = GetReservationByID(id);
            dbContext.Reservations.Remove(reservation);
        }

    }
}
