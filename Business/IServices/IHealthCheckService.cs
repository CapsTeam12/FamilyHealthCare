﻿using Contract.DTOs.HealthCheck;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IHealthCheckService
    {
        public Task<HealthCheckDto> HealthCheckAsync(HealthCheckDto healthCheckDto);
        public Task<HealthCheckDto> HealthCheckResultAsync(string accountId);
        public Task<List<HealthCheckDto>> HealthCheckListAsync(string accountId);
        public Task<HealthCheckDto> HealthCheckDetailsAsync(int id);
    }
}