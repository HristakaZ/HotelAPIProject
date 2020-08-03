using DataStructure;
using Hotel_API_Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EmployeeApplicationUser> GetEmployees()
        {
            return dbContext.Employees.ToList();
        }

        public EmployeeApplicationUser GetEmployeeByID(int id)
        {
            return dbContext.Employees.Where(x => x.Id == id).FirstOrDefault();
        }

        public void CreateEmployee(EmployeeApplicationUser employee)
        {
            dbContext.Employees.Add(employee);
        }

        public void UpdateEmployee(EmployeeApplicationUser employee)
        {
            EmployeeApplicationUser employeeToUpdate = GetEmployeeByID(employee.Id);
            // this code might be extended later, for more property updates
            employeeToUpdate.UserName = employee.UserName;
            dbContext.Employees.Update(employeeToUpdate);
        }

        public void DeleteEmployee(int id)
        {
            EmployeeApplicationUser employee = GetEmployeeByID(id);
            dbContext.Employees.Remove(employee);
        }

    }
}
