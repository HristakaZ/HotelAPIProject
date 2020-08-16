using DataStructure;
using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext dbContext;
        public PositionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<PositionApplicationRole> GetPositions()
        {
            return dbContext.Positions.ToList();
        }

        public PositionApplicationRole GetPositionByID(int id)
        {
            return dbContext.Positions.Where(x => x.Id == id).FirstOrDefault();
        }

        public void CreatePosition(PositionApplicationRole position)
        {
            dbContext.Positions.Add(position);
        }

        public void UpdatePosition(PositionApplicationRole position)
        {
            PositionApplicationRole positionToUpdate = GetPositionByID(position.Id);
            if (positionToUpdate != null)
            {
                // this code might be extended later, for more property updates
                positionToUpdate.Name = position.Name;
                dbContext.Positions.Update(positionToUpdate);
            }
        }

        public void DeletePosition(int id)
        {
            PositionApplicationRole position = GetPositionByID(id);
            if (position != null)
            {
                dbContext.Positions.Remove(position);
            }
        }

    }
}
