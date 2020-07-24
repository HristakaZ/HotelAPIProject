using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class Reservation
    {
        [Key]
        public int ID { get; set; }
        public Guest Guest { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RoomReservation> RoomReservations { get; set; }
    }
}
