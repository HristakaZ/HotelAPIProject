using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.ViewModels
{
    public class CreatePositionViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
