using AutoMapper;
using Business.IServices;
using Contract.DTOs.HealthCheck;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClsHealthCheckService : ControllerBase, IHealthCheckService
    {
        private readonly IBaseRepository<HealthCheck> _healthCheckRepos;
        private readonly IBaseRepository<Patient> _patientRepos;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ClsHealthCheckService(IMapper mapper,
                                    UserManager<User> userManager,
                                    IBaseRepository<Patient> patientRepos,
                                    IBaseRepository<HealthCheck> healthCheckRepos)
        {
            _userManager = userManager;
            _patientRepos = patientRepos;
            _healthCheckRepos = healthCheckRepos;
            _mapper = mapper;
        }
        public async Task<HealthCheckDto> HealthCheckAsync(HealthCheckDto healthCheckDto)
        {
            var user = await _userManager.FindByIdAsync(healthCheckDto.UserId);
            
            HealthCheck healthCheck = new HealthCheck
            {
                Age = healthCheckDto.Age,
                Height = healthCheckDto.Height,
                Weight = healthCheckDto.Weight,
                BloodPressure = healthCheckDto.BloodPressure,
                HeartRate = healthCheckDto.HeartRate,
                UserId = user.Id,
                Date = DateTime.Now,
                BMI = healthCheckDto.Weight/Math.Pow(healthCheckDto.Height/100, 2)
            };

            //var healthChecks = _mapper.Map<HealthCheck>(healthCheckDto);
            var newHealthCheck = await _healthCheckRepos.Create(healthCheck);
            var healthChecksDto = _mapper.Map<HealthCheckDto>(newHealthCheck);
            return healthChecksDto;
        }

        public async Task<HealthCheckDto> HealthCheckDetailsAsync(int id)
        {
            var healthCheck = await _healthCheckRepos
                               .Entities
                               .Where(h => h.Id == id)
                               .Include(p => p.User)
                               .FirstOrDefaultAsync();
            if (healthCheck != null)
            {
                var healthCheckDto = _mapper.Map<HealthCheckDto>(healthCheck);
                return healthCheckDto;
            }
            return null;
        }

        public async Task<List<HealthCheckDto>> HealthCheckListAsync(string accountId)
        {
            var healthChecks = await _healthCheckRepos
                                .Entities
                                .Where(a => a.UserId == accountId)
                                .Include(p => p.User)
                                .ToListAsync();
            var healthCheckDto = _mapper.Map<List<HealthCheckDto>>(healthChecks);
            return healthCheckDto;
        }

        public async Task<HealthCheckDto> HealthCheckResultAsync(string accountId)
        {
            var healthChecks = await _healthCheckRepos
                                .Entities
                                .Where(a => a.UserId == accountId)
                                .Include(p => p.User)
                                .OrderByDescending(h => h.Date)
                                .FirstOrDefaultAsync();
            var healthCheckDto = _mapper.Map<HealthCheckDto>(healthChecks);
            return healthCheckDto;
        }
    }
}
