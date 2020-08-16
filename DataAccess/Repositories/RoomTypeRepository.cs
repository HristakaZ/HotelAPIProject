using DataStructure;
using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly ApplicationDbContext dbContext;
        public RoomTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<RoomType> GetRoomTypes()
        {
            return dbContext.RoomTypes.ToList();
        }

        public RoomType GetRoomTypeByID(int id)
        {
            return dbContext.RoomTypes.Where(x => x.ID == id).FirstOrDefault();
        }

        public void CreateRoomType(RoomType roomType)
        {
            dbContext.RoomTypes.Add(roomType);
        }

        public void UpdateRoomType(RoomType roomType)
        {
            RoomType roomTypeToUpdate = GetRoomTypeByID(roomType.ID);
            if (roomTypeToUpdate != null)
            {
                // this code might be extended later, for more property updates
                roomTypeToUpdate.Name = roomType.Name;
                dbContext.RoomTypes.Update(roomTypeToUpdate);
            }
        }

        public void DeleteRoomType(int id)
        {
            RoomType roomType = GetRoomTypeByID(id);
            if (roomType != null)
            {
                dbContext.RoomTypes.Remove(roomType);
            }
        }

    }
}
