﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_API_Project.Controllers
{
    public class ReservationController : Controller
    {
        // GET: ReservationController
        public ActionResult Index()
        {
            return View();
        }
    }
}
