using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStructure
{
    public class RoomType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
