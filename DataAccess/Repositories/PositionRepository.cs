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

        public List<Position> GetPositions()
        {
            return dbContext.Positions.ToList();
        }

        public Position GetPositionByID(int id)
        {
            return dbContext.Positions.Where(x => x.ID == id).FirstOrDefault();
        }

        public void CreatePosition(Position position)
        {
            dbContext.Positions.Add(position);
        }

        public void UpdatePosition(int id)
        {
            Position position = GetPositionByID(id);
            dbContext.Positions.Update(position);
        }

        public void DeletePosition(int id)
        {
            Position position = GetPositionByID(id);
            dbContext.Positions.Remove(position);
        }

    }
}
