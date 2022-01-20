using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class FavouriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
