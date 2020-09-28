using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStructure
{
    public class EmployeeApplicationUser : IdentityUser<int>
    {
        [JsonProperty(Order = -1)]
        public override int Id { get => base.Id; set => base.Id = value; }
        public virtual PositionApplicationRole Position { get; set; }
        [JsonIgnore]
        public virtual List<Reservation> Reservations { get; set; }
    }
}
