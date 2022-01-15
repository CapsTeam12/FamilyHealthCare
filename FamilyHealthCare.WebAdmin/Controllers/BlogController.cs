using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.WebAdmin.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBlog()
        {
            return View();
        }
        public IActionResult EditBlog()
        {
            return View();
        }
        public IActionResult DetailsBlog()
        {
            return View();
        }
    }
}
