using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IRoomRepository
    {
        List<Room> GetRooms();
        Room GetRoomByID(int id);
        void CreateRoom(Room room);
        void UpdateRoom(int id);
        void DeleteRoom(int id);
    }
}
