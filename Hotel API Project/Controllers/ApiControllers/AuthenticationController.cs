using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories;
using DataStructure;
using Hotel_API_Project.Mappers;
using Hotel_API_Project.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IEmployeeRepository iEmployeeRepository;
        private IPositionRepository iPositionRepository;
        private UserManager<EmployeeApplicationUser> userManager;
        public AuthenticationController(IEmployeeRepository iEmployeeRepository,
            UserManager<EmployeeApplicationUser> userManager,
            IPositionRepository iPositionRepository)
        {
            this.iEmployeeRepository = iEmployeeRepository;
            this.userManager = userManager;
            this.iPositionRepository = iPositionRepository;
        }
        [HttpPost]
        public IActionResult Login(EmployeeViewModel employeeViewModel)
        {
            //TO DO: make a try-catch block afterwards(what if the employee is null ???)
            //TO DO: you can make an overload of GetEmployees(adding an optional Func<> as a select(so as to use it as a select)) in the employee repository
            EmployeeApplicationUser employee = new EmployeeApplicationUser();
            employee = iEmployeeRepository.GetEmployees().Where(x => x.UserName == employeeViewModel.UserName).FirstOrDefault();
            PasswordVerificationResult isNormalPasswordEqualToHashedPassword =
                userManager.PasswordHasher.VerifyHashedPassword(employee, employee.PasswordHash, employeeViewModel.NormalPassword);
            List<EmployeeApplicationUser> employees = iEmployeeRepository.GetEmployees();
            if (isNormalPasswordEqualToHashedPassword != PasswordVerificationResult.Failed)
            {
                // password is correct 
                if (employees.Exists(x => x.UserName == employee.UserName && x.PasswordHash == employee.PasswordHash))
                {
                    SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    SigningCredentials signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                        new Claim(ClaimTypes.Name, employee.UserName)
                    };
                    if (employee.Position != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, employee.Position.Name));
                    }
                    JwtSecurityToken tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signInCredentials
                    );
                    string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return Ok(new { Token = tokenString });
                }
            }
            return NotFound("Wrong username/password!");
        }
    }
}
