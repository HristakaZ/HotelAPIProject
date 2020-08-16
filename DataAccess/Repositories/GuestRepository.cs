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

        public void UpdateGuest(Guest guest)
        {
            Guest guestToUpdate = GetGuestByID(guest.ID);
            if (guestToUpdate != null)
            {
                // this code might be extended later, for more property updates
                guestToUpdate.Name = guest.Name;
                dbContext.Guests.Update(guestToUpdate);
            }
        }

        public void DeleteGuest(int id)
        {
            Guest guest = GetGuestByID(id);
            if (guest != null)
            {
                dbContext.Guests.Remove(guest);
            }
        }

    }
}
