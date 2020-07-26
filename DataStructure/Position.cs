using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class Position
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<Employee> Employee { get; set; }
    }
}
