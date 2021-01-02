using DataStructure;
using Hotel_API_Project.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class CreateReservationViewModel
    {
        public int ID { get; set; }
        [Required]
        [StartEndDateValidator]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public virtual Guest Guest { get; set; }
        [Required]
        public virtual EmployeeApplicationUser Employee { get; set; }
        [Required]
        public virtual List<RoomReservation> RoomReservations { get; set; }
    }
}
