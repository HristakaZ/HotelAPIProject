using DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IEmployeeRepository
    {
        List<EmployeeApplicationUser> GetEmployees();
        EmployeeApplicationUser GetEmployeeByID(int id);
        void CreateEmployee(EmployeeApplicationUser employee);
        void UpdateEmployee(int id);
        void DeleteEmployee(int id);
    }
}
