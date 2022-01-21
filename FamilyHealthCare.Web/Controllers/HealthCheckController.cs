using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class HealthCheckController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HealthCheckSuccessfully()
        {
            return View();
        }
        public IActionResult HealthCheckDetails()
        {
            return View();
        }
    }
}
