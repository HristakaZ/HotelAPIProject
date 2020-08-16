using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Hotel_API_Project.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository iEmployeeRepository;
        private IUnitOfWork iUnitOfWork;
        public EmployeeController(IEmployeeRepository iEmployeeRepository, IUnitOfWork iUnitOfWork)
        {
            this.iEmployeeRepository = iEmployeeRepository;
            this.iUnitOfWork = iUnitOfWork;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public List<EmployeeApplicationUser> GetEmployees()
        {
            List<EmployeeApplicationUser> employees = iEmployeeRepository.GetEmployees();
            return employees;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}", Name = "GetEmployeeByID")]
        public IActionResult GetEmployeeByID(int id)
        {
            EmployeeApplicationUser employee = iEmployeeRepository.GetEmployeeByID(id);
            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return NotFound("Employee with ID " + id.ToString() + " was not found.");
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public IActionResult Post([FromBody] EmployeeApplicationUser employee)
        {
            try
            {
                iEmployeeRepository.CreateEmployee(employee);
                Uri uri = new Uri(Url.Link("GetEmployeeByID", new { Id = employee.Id }));
                iUnitOfWork.Save();
                return Created(uri, employee.Id.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EmployeeApplicationUser employee)
        {
            if (employee != null)
            {
                employee.Id = id;
                iEmployeeRepository.UpdateEmployee(employee);
                iUnitOfWork.Save();
                return Ok(employee);
            }
            else
            {
                return NotFound("Employee with ID " + id.ToString() + " was not found.");
            }
        }


        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            EmployeeApplicationUser employeeToDelete = iEmployeeRepository.GetEmployeeByID(id);
            if (employeeToDelete != null)
            {
                iEmployeeRepository.DeleteEmployee(employeeToDelete.Id);
                iUnitOfWork.Save();
                return Ok(employeeToDelete);
            }
            else
            {
                return NotFound("Employee with ID " + id.ToString() + " was not found.");
            }
        }
    }
}