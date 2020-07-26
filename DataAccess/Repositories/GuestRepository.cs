using DataStructure;
using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDbContext dbContext;
        public GuestRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Guest> GetGuests()
        {
            return dbContext.Guests.ToList();
        }

        public Guest GetGuestByID(int id)
        {
            return dbContext.Guests.Where(x => x.ID == id).FirstOrDefault();
        }

        public void CreateGuest(Guest guest)
        {
            dbContext.Guests.Add(guest);
        }

        public void UpdateGuest(int id)
        {
            Guest guest = GetGuestByID(id);
            dbContext.Guests.Update(guest);
        }

        public void DeleteGuest(int id)
        {
            Guest guest = GetGuestByID(id);
            dbContext.Guests.Remove(guest);
        }

    }
}
