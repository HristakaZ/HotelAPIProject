using DataAccess.Repositories;
using DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Services
{
    public class UpdateRoomValidationService : IUpdateRoomValidationService
    {
        private IRoomTypeRepository iRoomTypeRepository;
        private IRoomRepository iRoomRepository;
        public UpdateRoomValidationService(IRoomTypeRepository iRoomTypeRepository, IRoomRepository iRoomRepository)
        {
            this.iRoomTypeRepository = iRoomTypeRepository;
            this.iRoomRepository = iRoomRepository;
        }
        public void UpdateRoomValidation(Room room)
        {
            if (room.RoomType.ID != 0)
            {
                RoomType roomTypeFromDropDownList = iRoomTypeRepository.GetRoomTypeByID(room.RoomType.ID);
                room.RoomType = roomTypeFromDropDownList;
            }
            else
            {
                room.RoomType = iRoomRepository.GetRoomByID(room.ID).RoomType;
            }
            if (room.Number == 0)
            {
                room.Number = iRoomRepository.GetRoomByID(room.ID).Number;
            }
            else
            {
                room.Number = room.Number;
            }
        }
    }
}
