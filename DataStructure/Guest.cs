using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class Guest
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
