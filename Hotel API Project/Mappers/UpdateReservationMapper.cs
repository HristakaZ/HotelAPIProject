using DataStructure;
using Hotel_API_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Mappers
{
    public class UpdateReservationMapper : IUpdateReservationMapper
    {
        public Reservation MapUpdateReservationViewModelToModel(UpdateReservationViewModel updateReservationViewModel, Reservation reservation)
        {
            reservation.ID = updateReservationViewModel.ID;
            if (updateReservationViewModel.StartDate.HasValue && updateReservationViewModel.EndDate.HasValue)
            {
                reservation.StartDate = (DateTime)updateReservationViewModel.StartDate;
                reservation.EndDate = (DateTime)updateReservationViewModel.EndDate;
            }
            reservation.Guest = updateReservationViewModel.Guest;
            reservation.Employee = updateReservationViewModel.Employee;
            reservation.RoomReservations = updateReservationViewModel.RoomReservations;
            if (reservation.RoomReservations != null)
            {
                foreach (RoomReservation roomReservation in reservation.RoomReservations)
                {
                    if (roomReservation != null)
                    {
                        roomReservation.ReservationID = updateReservationViewModel.ID;
                    }
                }
            }
            return reservation;
        }
    }
}
