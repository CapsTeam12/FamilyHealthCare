using FamilyHealthCare.Doctor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpContext = httpContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.AccountId = accountId;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
