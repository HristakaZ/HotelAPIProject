using DataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class ReservationViewModel
    {
        [Key]
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual EmployeeApplicationUser Employee { get; set; }
        public virtual RoomReservation RoomReservation { get; set; }
    }
}
