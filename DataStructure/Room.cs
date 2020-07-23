using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    public class Room
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public RoomType RoomType { get; set; }
        public List<RoomReservation> RoomReservations { get; set; }
    }
}
