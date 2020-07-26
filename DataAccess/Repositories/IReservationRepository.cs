﻿using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> GetReservations();
        Reservation GetReservationByID(int id);
        void CreateReservation(Reservation reservation);
        void UpdateReservation(int id);
        void DeleteReservation(int id);
    }
}
