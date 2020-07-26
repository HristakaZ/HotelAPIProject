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

        public List<Employee> GetEmployees()
        {
            return dbContext.Employees.ToList();
        }

        public Employee GetEmployeeByID(int id)
        {
            return dbContext.Employees.Where(x => x.ID == id).FirstOrDefault();
        }

        public void CreateEmployee(Employee employee)
        {
            dbContext.Add(employee);
        }

        public void UpdateEmployee(int id)
        {
            Employee employee = GetEmployeeByID(id);
            dbContext.Employees.Update(employee);
        }

        public void DeleteEmployee(int id)
        {
            Employee employee = GetEmployeeByID(id);
            dbContext.Employees.Remove(employee);
        }

    }
}
