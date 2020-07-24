using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStructure
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
