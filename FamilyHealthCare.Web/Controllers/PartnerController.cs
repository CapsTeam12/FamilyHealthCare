﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class PartnerController : Controller
    {
        public IActionResult DoctorRegister()
        {
            return View();
        }
        public IActionResult PharmacyRegister()
        {
            return View();
        }
    }
}
