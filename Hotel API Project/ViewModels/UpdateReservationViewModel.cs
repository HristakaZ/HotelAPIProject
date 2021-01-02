using DataStructure;
using Hotel_API_Project.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class UpdateReservationViewModel
    {
        public int ID { get; set; }
        [StartEndDateValidator]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual EmployeeApplicationUser Employee { get; set; }
        public virtual List<RoomReservation> RoomReservations { get; set; }
    }
}
