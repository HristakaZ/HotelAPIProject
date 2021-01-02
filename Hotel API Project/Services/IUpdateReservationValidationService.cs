using DataStructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Services
{
    public interface IUpdateReservationValidationService
    {
        public void UpdateReservationValidation(Reservation reservation);
    }
}
