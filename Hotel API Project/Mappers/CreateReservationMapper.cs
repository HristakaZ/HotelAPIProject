using DataStructure;
using Hotel_API_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Mappers
{
    public class CreateReservationMapper : ICreateReservationMapper
    {
        public Reservation MapCreateReservationViewModelToModel(CreateReservationViewModel createReservationViewModel, Reservation reservation)
        {
            reservation.ID = createReservationViewModel.ID;
            reservation.StartDate = createReservationViewModel.StartDate;
            reservation.EndDate = createReservationViewModel.EndDate;
            reservation.Guest = createReservationViewModel.Guest;
            reservation.Employee = createReservationViewModel.Employee;
            reservation.RoomReservations = createReservationViewModel.RoomReservations;
            if (reservation.RoomReservations != null)
            {
                foreach (RoomReservation roomReservation in reservation.RoomReservations)
                {
                    roomReservation.ReservationID = createReservationViewModel.ID;
                }
            }
            return reservation;
        }
    }
}
