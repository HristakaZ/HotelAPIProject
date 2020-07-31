using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStructure
{
    public class EmployeeApplicationUser : IdentityUser<int>
    {
        public virtual PositionApplicationRole Position { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
