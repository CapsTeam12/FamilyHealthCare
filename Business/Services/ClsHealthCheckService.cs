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
            //var patient = await _patientRepos.
            //                Entities.
            //                FirstOrDefaultAsync(x => x.AccountId == patientId);
            var user = await _userManager.FindByIdAsync(healthCheckDto.PatientId);
            //if (user == null)
            //    return Unauthorized();
            HealthCheck healthCheck = new HealthCheck
            {
                Age = healthCheckDto.Age,
                Height = healthCheckDto.Height,
                Weight = healthCheckDto.Weight,
                BloodPressure = healthCheckDto.BloodPressure,
                HeartRate = healthCheckDto.HeartRate,
                PatientId = user.ToString(),
                Date = DateTime.Now,
                BMI = healthCheckDto.Weight/Math.Pow(healthCheckDto.Height/100, 2)
            };

            //var healthChecks = _mapper.Map<HealthCheck>(healthCheckDto);
            var newHealthCheck = await _healthCheckRepos.Create(healthCheck);
            var healthChecksDto = _mapper.Map<HealthCheckDto>(newHealthCheck);
            return healthChecksDto;
        }

        public Task<HealthCheckDto> HealthCheckResultAsync(HealthCheckDto healthCheckDto)
        {
            throw new NotImplementedException();
        }

        //public Task<HealthCheckDto> HealthCheckResultAsync(HealthCheckDto healthCheckDto)
        //{
        //var patient = _patientRepos
        //                  .Entities
        //                  .Include(a => a.User)
        //                  .Where(x => x.AccountId == id)
        //                  .FirstOrDefault();
        //var patientsDto = _mapper.Map<PatientDetailsDto>(patient);
        //patientsDto.Email = patient.User.Email;
        //return healthChecksDto;
        //}
    }
}
