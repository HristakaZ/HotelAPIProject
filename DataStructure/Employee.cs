using System;
using System.Collections.Generic;

namespace DataStructure
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
