using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DataStructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataAccess.Repositories;
using Hotel_API_Project.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Hotel_API_Project.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;

namespace Hotel_API_Project.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<EmployeeApplicationUser> _userManager;
        private readonly SignInManager<EmployeeApplicationUser> _signInManager;
        private readonly ILogger<EmployeeApplicationUser> _logger;
        private IEmployeeRepository iEmployeeRepository;
        public LoginModel(SignInManager<EmployeeApplicationUser> signInManager,
            ILogger<EmployeeApplicationUser> logger,
            UserManager<EmployeeApplicationUser> userManager,
            IEmployeeRepository iEmployeeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this.iEmployeeRepository = iEmployeeRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel : EmployeeApplicationUser
        {
            [Required]
            [DataType(DataType.Password)]
            public string NormalPassword { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                /*var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.NormalPassword, Input.RememberMe, lockoutOnFailure: false);*/
                HttpClient httpClient = new HttpClient();
                InputModel inputModelEmployee = new InputModel()
                {
                    UserName = Input.UserName,
                    NormalPassword = Input.NormalPassword
                };
                string json = JsonConvert.SerializeObject(inputModelEmployee);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("https://localhost:44357/api/Authentication", data);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("User logged in.");
                    string jsonToken = await response.Content.ReadAsStringAsync();
                    TokenDTO token = JsonConvert.DeserializeObject<TokenDTO>(jsonToken);
                    string jsonWebToken = token.Token;
                    HttpContext.Response.Cookies.Append("JWTCookie", jsonWebToken, new CookieOptions() { HttpOnly = true, Secure = true});
                    returnUrl = "/Home/Index";
                    SecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken decodedJsonWebToken = handler.ReadToken(jsonWebToken) as JwtSecurityToken;
                    string userIDClaim = decodedJsonWebToken.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault().Value;
                    EmployeeApplicationUser currentEmployeeUser = iEmployeeRepository.GetEmployeeByID(int.Parse(userIDClaim));
                    bool isAuthenticated = currentEmployeeUser.Id != 0;
                    HttpContext.Session.SetString("IsAuthenticated", isAuthenticated.ToString().ToLower());
                    HttpContext.Session.SetString("UserName", currentEmployeeUser.UserName);
                    HttpContext.Session.SetString("Role", currentEmployeeUser.Position.Name);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
