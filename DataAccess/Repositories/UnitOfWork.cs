using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
