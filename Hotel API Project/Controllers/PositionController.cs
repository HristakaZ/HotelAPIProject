using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_API_Project.Controllers
{
    public class PositionController : Controller
    {
        // GET: PositionController
        public ActionResult Index()
        {
            return View();
        }
    }
}
