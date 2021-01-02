using DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class UpdateEmployeeViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public PositionApplicationRole Position { get; set; }
    }
}
