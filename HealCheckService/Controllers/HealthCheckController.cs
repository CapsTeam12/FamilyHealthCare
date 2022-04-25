using Business.IServices;
using Contract.DTOs.HealthCheck;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealCheckService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;
        public HealthCheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }
        [HttpPost]
        [Route("healthcheck")]
        public async Task<HealthCheckDto> HealthCheck([FromBody] HealthCheckDto healthCheckDto)
        {

            var healthCheck = await _healthCheckService.HealthCheckAsync(healthCheckDto);
            return healthCheck;
        }
    }
    
}
