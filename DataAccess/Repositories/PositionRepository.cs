﻿using DataStructure;
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

        public void UpdatePosition(int id)
        {
            PositionApplicationRole position = GetPositionByID(id);
            dbContext.Positions.Update(position);
        }

        public void DeletePosition(int id)
        {
            PositionApplicationRole position = GetPositionByID(id);
            dbContext.Positions.Remove(position);
        }

    }
}
