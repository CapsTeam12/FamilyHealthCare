using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.WebAdmin.Controllers
{
    public class PharmacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailsPharmacy()
        {
            return View();
        }
        public IActionResult RequestPharmacy()
        {
            return View();
        }
    }
}
