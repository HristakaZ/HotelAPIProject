using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IEmployeeRepository iEmployeeRepository;
        private IPositionRepository iPositionRepository;
        private UserManager<EmployeeApplicationUser> userManager;
        public IConfiguration Configuration { get; }
        public AuthenticationController(IEmployeeRepository iEmployeeRepository,
            UserManager<EmployeeApplicationUser> userManager,
            IPositionRepository iPositionRepository,
            IConfiguration configuration)
        {
            this.iEmployeeRepository = iEmployeeRepository;
            this.userManager = userManager;
            this.iPositionRepository = iPositionRepository;
            this.Configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login(EmployeeViewModel employeeViewModel)
        {
            EmployeeApplicationUser employee = new EmployeeApplicationUser();
            employee = iEmployeeRepository.GetEmployees().Where(x => x.UserName == employeeViewModel.UserName).FirstOrDefault();
            if (employee != null)
            {
                PasswordVerificationResult isNormalPasswordEqualToHashedPassword =
                    userManager.PasswordHasher.VerifyHashedPassword(employee, employee.PasswordHash, employeeViewModel.NormalPassword);
                List<EmployeeApplicationUser> employees = iEmployeeRepository.GetEmployees();
                if (isNormalPasswordEqualToHashedPassword != PasswordVerificationResult.Failed)
                {
                    // password is correct 
                    if (employees.Exists(x => x.UserName == employee.UserName && x.PasswordHash == employee.PasswordHash))
                    {
                        string secret = Configuration["JWTConfiguration:secret"];
                        string issuer = Configuration["JWTConfiguration:issuer"];
                        string audience = Configuration["JWTConfiguration:audience"];

                        SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
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
                            issuer: issuer,
                            audience: audience,
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(5),
                            signingCredentials: signInCredentials
                        );
                        string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                        return Ok(new { Token = tokenString });
                    }
                }
            }
            return NotFound("Wrong username/password!");
        }
    }
}
