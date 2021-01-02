using DataAccess.Repositories;
using DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Services
{
    public class UpdateReservationValidationService : IUpdateReservationValidationService
    {
        private IReservationRepository iReservationRepository;
        private IRoomRepository iRoomRepository;
        public UpdateReservationValidationService(IReservationRepository iReservationRepository, IRoomRepository iRoomRepository)
        {
            this.iReservationRepository = iReservationRepository;
            this.iRoomRepository = iRoomRepository;
        }
        public void UpdateReservationValidation(Reservation reservation)
        {
            reservation.StartDate = reservation.StartDate == default(DateTime) ?
                reservation.StartDate = iReservationRepository.GetReservationByID(reservation.ID).StartDate : reservation.StartDate;
            reservation.EndDate = reservation.EndDate == default(DateTime) ?
                reservation.EndDate = iReservationRepository.GetReservationByID(reservation.ID).EndDate : reservation.EndDate;
            reservation.Guest = reservation.Guest.ID == 0 ?
                reservation.Guest = iReservationRepository.GetReservationByID(reservation.ID).Guest : reservation.Guest;
            reservation.Employee = reservation.Employee.Id == 0 ?
                reservation.Employee = iReservationRepository.GetReservationByID(reservation.ID).Employee : reservation.Employee;
            if (reservation.RoomReservations != null)
            {
                reservation.RoomReservations.ForEach(x =>
                { 
                    //only checking the RoomReservations which are not null(the ones that we actually care for(valid input))
                    if (x != null)
                    {
                        //if no room is picked, the rooms remain the same as when created
                        if (x.RoomID == 0)
                        {
                            reservation.RoomReservations = iReservationRepository.GetReservationByID(reservation.ID).RoomReservations;
                        }
                        x.Room = iRoomRepository.GetRoomByID(x.RoomID);
                        Reservation reservationFromDB = iReservationRepository.GetReservationByID(x.ReservationID);
                        /*if the chosen rooms are the same, the rooms remain the same as when created(only if they're no more than 3 - that way we
                        /can add more rooms if we want to)*/
                        if (reservationFromDB.RoomReservations.Exists(y => y.Room == x.Room) && reservation.RoomReservations.Count < 3)
                        {
                            reservation.RoomReservations = iReservationRepository.GetReservationByID(reservation.ID).RoomReservations;
                        }
                    }
                });
            }
        }
    }
}
