using DataStructure;
using Hotel_API_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Mappers
{
    public class UpdateRoomMapper : IUpdateRoomMapper
    {
        public Room MapUpdateRoomViewModelToModel(UpdateRoomViewModel updateRoomViewModel, Room room)
        {
            room.ID = updateRoomViewModel.ID;
            room.Number = updateRoomViewModel.Number;
            room.RoomType = updateRoomViewModel.RoomType;
            room.RoomReservations = room.RoomReservations;
            return room;
        }
    }
}
