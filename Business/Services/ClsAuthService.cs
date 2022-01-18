using AutoMapper;
using Business.IServices;
using Contract.DTOs.AuthService;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Business
{
    public class ClsAuthService : ControllerBase, IAuthService
    {
        //private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<Doctor> _doctorRepos;
        private readonly IBaseRepository<Pharmacy> _pharmacyRepos;
        private readonly IBaseRepository<Patient> _patientRepos;
        private readonly IMapper _mapper;

        public ClsAuthService( IMapper mapper,
                                IBaseRepository<Doctor> doctorRepos,
                                IBaseRepository<Pharmacy> pharmacyRepos,
                                IBaseRepository<Patient> patientRepos)
        {
            //_userManager = userManager;
            _doctorRepos = doctorRepos;
            _pharmacyRepos = pharmacyRepos;
            _patientRepos = patientRepos;
            _mapper = mapper;
        }
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //var changePasswordDtos = _mapper.Map<User>(changePasswordDto);

            //var userIdentity = await _userManager.FindByIdAsync(user.Id);
            //await _userManager.UpdateAsync(user.Id, changePasswordDtos);
            //user.PasswordHash = await PasswordHasher.HashPassword(changePasswordDto.NewPassword);
            //var result = await _userManager.UpdateAsync(user);
            return Ok();
        }

        public Task<IActionResult> UpdateAdminProfileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateDoctorProfileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdatePatientProfileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdatePharmacyProfileAsync()
        {
            throw new NotImplementedException();
        }
    }
}
