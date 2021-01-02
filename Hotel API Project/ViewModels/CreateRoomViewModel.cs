using DataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class CreateRoomViewModel
    {
        [Key]
        public int ID { get; set; }
        public int Number { get; set; }
        public virtual RoomType RoomType { get; set; }
        [JsonIgnore]
        public virtual List<RoomReservation> RoomReservations { get; set; }
    }
}
