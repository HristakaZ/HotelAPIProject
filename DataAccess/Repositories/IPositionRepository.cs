using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IPositionRepository
    {
        List<PositionApplicationRole> GetPositions();
        PositionApplicationRole GetPositionByID(int id);
        void CreatePosition(PositionApplicationRole position);
        void UpdatePosition(PositionApplicationRole position);
        void DeletePosition(int id);
    }
}
