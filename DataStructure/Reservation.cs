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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual EmployeeApplicationUser Employee { get; set; }
        public virtual List<RoomReservation> RoomReservations { get; set; }
    }
}
