using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET: api/<EmployeeController>
        [HttpGet]
        public List<Employee> GetEmployees()
        {
            return employees;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}", Name = "GetEmployeeByID")]
        public IActionResult GetEmployeeByID(int id)
        {
            Employee employee = employees.Where(x => x.ID == id).FirstOrDefault();
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
        public IActionResult Post([FromBody] Employee employee)
        {
            try
            {
                employees.Add(employee);
                Uri uri = new Uri(Url.Link("GetEmployeeByID", new { id = employee.ID }));
                return Created(uri, employee.ID.ToString());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), BadRequest().ToString());
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Employee employee)
        {
            Employee employeeToUpdate = employees.Where(x => x.ID == id).FirstOrDefault();
            if (employeeToUpdate != null)
            {
                employeeToUpdate.ID = employee.ID;
                employeeToUpdate.Name = employee.Name;
                return Ok(employeeToUpdate);
            }
            else
            {
                return NotFound("Employee with ID " + employee.ID.ToString() + " was not found.");
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Employee employeeToDelete = employees.Where(x => x.ID == id).FirstOrDefault();
            if (employeeToDelete != null)
            {
                employees.Remove(employeeToDelete);
                return Ok(employeeToDelete);
            }
            else
            {
                return NotFound("Employee with ID " + id.ToString() + " was not found.");
            }
        }
    }
}
*/