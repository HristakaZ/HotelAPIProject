using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetRoomTypes();
        RoomType GetRoomTypeByID(int id);
        void CreateRoomType(RoomType roomType);
        void UpdateRoomType(RoomType roomType);
        void DeleteRoomType(int id);
    }
}
