using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_API_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XSRFController : ControllerBase
    {
        private readonly IAntiforgery iAntiForgery;

        public XSRFController(IAntiforgery iAntiForgery)
        {
            this.iAntiForgery = iAntiForgery;
        }

        public IActionResult GetXSRFToken()
        {
            AntiforgeryTokenSet tokens = iAntiForgery.GetAndStoreTokens(HttpContext);

            return new ObjectResult(new
            {
                token = tokens.RequestToken,
                tokenHeader = tokens.HeaderName
            });
        }
    }
}
