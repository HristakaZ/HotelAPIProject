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
        private UserManager<EmployeeApplicationUser> userManager;
        public AuthenticationController(IEmployeeRepository iEmployeeRepository,
            UserManager<EmployeeApplicationUser> userManager)
        {
            this.iEmployeeRepository = iEmployeeRepository;
            this.userManager = userManager;
        }
        [HttpPost(Name = "Login")]
        public IActionResult Login(EmployeeViewModel employeeViewModel)
        {
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
                    JwtSecurityToken tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(),
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
