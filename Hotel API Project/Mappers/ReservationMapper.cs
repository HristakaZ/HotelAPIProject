using DataStructure;
using Hotel_API_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Mappers
{
    public class ReservationMapper : IReservationMapper
    {
        public Reservation MapReservationViewModelToModel(ReservationViewModel reservationViewModel, Reservation reservation)
        {
            reservation.ID = reservationViewModel.ID;
            reservation.StartDate = reservationViewModel.StartDate;
            reservation.EndDate = reservationViewModel.EndDate;
            reservation.Guest = reservationViewModel.Guest;
            reservation.Employee = reservationViewModel.Employee;
            return reservation;
        }
    }
}
