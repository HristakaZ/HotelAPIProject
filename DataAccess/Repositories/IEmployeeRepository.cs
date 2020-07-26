using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees();
        Employee GetEmployeeByID(int id);
        void CreateEmployee(Employee employee);
        void UpdateEmployee(int id);
        void DeleteEmployee(int id);
    }
}
