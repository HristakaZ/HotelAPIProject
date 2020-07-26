using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IPositionRepository
    {
        List<Position> GetPositions();
        Position GetPositionByID(int id);
        void CreatePosition(Position position);
        void UpdatePosition(int id);
        void DeletePosition(int id);
    }
}
