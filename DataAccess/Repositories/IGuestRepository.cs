using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IGuestRepository
    {
        List<Guest> GetGuests();
        Guest GetGuestByID(int id);
        void CreateGuest(Guest guest);
        void UpdateGuest(Guest guest);
        void DeleteGuest(int id);
    }
}
