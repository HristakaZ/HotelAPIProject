using DataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class CreateRoomTypeViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual List<Room> Rooms { get; set; }
    }
}
