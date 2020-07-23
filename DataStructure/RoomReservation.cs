using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    public class RoomReservation
    {
        public int RoomID { get; set; }
        public Room Room { get; set; }
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }
    }
}
