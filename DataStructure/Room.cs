using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class Room
    {
        [Key]
        public int ID { get; set; }
        public int Number { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual List<RoomReservation> RoomReservations { get; set; }
    }
}
