using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetRoomType();
        RoomType GetRoomTypeByID(int id);
        void CreateRoomType(RoomType roomType);
        void UpdateRoomType(int id);
        void DeleteRoomType(int id);
    }
}
