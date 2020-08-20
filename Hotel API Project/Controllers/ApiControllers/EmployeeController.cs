﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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
        private HtmlEncoder htmlEncoder;
        public EmployeeController(IEmployeeRepository iEmployeeRepository, IUnitOfWork iUnitOfWork, HtmlEncoder htmlEncoder)
        {
            this.iEmployeeRepository = iEmployeeRepository;
            this.iUnitOfWork = iUnitOfWork;
            this.htmlEncoder = htmlEncoder;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public List<EmployeeApplicationUser> GetEmployees()
        {
            List<EmployeeApplicationUser> employees = iEmployeeRepository.GetEmployees();
            /*encoding(against xss) each username at the get request, so as to store the entity column in its plain form in the database
             note: this can be extended to a greater extent(encode other properties besides the username)*/
            employees.ForEach(x => {
                string encodedEmployeeUserName = htmlEncoder.Encode(x.UserName);
                x.UserName = encodedEmployeeUserName;
            });
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