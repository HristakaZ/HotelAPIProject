using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class PositionApplicationRole : IdentityRole<int>
    {
        public virtual List<EmployeeApplicationUser> Employees { get; set; }
    }
}
