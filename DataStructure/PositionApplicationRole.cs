using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class PositionApplicationRole : IdentityRole<int>
    {
        [JsonIgnore]
        public virtual List<EmployeeApplicationUser> Employees { get; set; }
    }
}
