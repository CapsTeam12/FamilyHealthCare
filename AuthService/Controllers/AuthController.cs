using Contract.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("test")]
        [Authorize(Policy = SecurityConstants.ADMIN_ROLE_POLICY)]
        public string Test()
        {
            return "connect successfully";
        }
    }
}
