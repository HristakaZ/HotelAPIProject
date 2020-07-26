using DataStructure;
using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext dbContext;
        public RoomRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Room> GetRooms()
        {
            return dbContext.Rooms.ToList();
        }

        public Room GetRoomByID(int id)
        {
            return dbContext.Rooms.Where(x => x.ID == id).FirstOrDefault();
        }

        public void CreateRoom(Room room)
        {
            dbContext.Rooms.Add(room);
        }

        public void UpdateRoom(int id)
        {
            Room room = GetRoomByID(id);
            dbContext.Rooms.Update(room);
        }

        public void DeleteRoom(int id)
        {
            Room room = GetRoomByID(id);
            dbContext.Rooms.Remove(room);
        }

    }
}
