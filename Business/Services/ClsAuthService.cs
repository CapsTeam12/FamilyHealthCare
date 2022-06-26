using AutoMapper;
using Business.IServices;
using Business.Services;
using Contract.DTOs.AuthService;
using Contract.DTOs.ManagementService;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{
    public class ClsAuthService : ControllerBase, IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<User> _userRepos;
        private readonly IBaseRepository<Doctor> _doctorRepos;
        private readonly IBaseRepository<Pharmacy> _pharmacyRepos;
        private readonly IBaseRepository<Patient> _patientRepos;
        private readonly IBaseRepository<Specialities> _specializedRepos;
        private readonly IFileService _fileService;

        private readonly IMapper _mapper;

        public ClsAuthService(IBaseRepository<User> userRepos, IMapper mapper,
                                UserManager<User> userManager,
                                IBaseRepository<Doctor> doctorRepos,
                                IBaseRepository<Pharmacy> pharmacyRepos,
                                IBaseRepository<Patient> patientRepos,
                                IBaseRepository<Specialities> specializedRepos,
                                IFileService fileService)
        {
            _userManager = userManager;
            _userRepos = userRepos;
            _doctorRepos = doctorRepos;
            _pharmacyRepos = pharmacyRepos;
            _patientRepos = patientRepos;
            _fileService = fileService; 
            _mapper = mapper;
        }
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDto.UserId);
            if (user == null)
                return Unauthorized();
            var checkPass = await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
            var validPass = await _userManager.CheckPasswordAsync(user, changePasswordDto.NewPassword);
            if (!checkPass)
                return NoContent();
            if (validPass)
                return NotFound();
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if(result.Succeeded)
                return Ok(result);
            return BadRequest();
        }

        public async Task<IActionResult> UpdateAdminProfileAsync()
        {
            return Ok();
        }

        public async Task<IActionResult> UpdateDoctorProfileAsync([FromForm] DoctorUpdateDto doctorDetailsDto)
        {
            var user = _doctorRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.AccountId == doctorDetailsDto.AccountId)
                                .First();
            if (user == null)
                return Unauthorized();
            user.FullName = doctorDetailsDto.FullName;
            user.Phone = doctorDetailsDto.Phone;
            user.Gender = (int)doctorDetailsDto.Gender;
            user.DateOfBirth = doctorDetailsDto.DateOfBirth;
            user.Address = doctorDetailsDto.Address;
            if (doctorDetailsDto.Avatar != null)
            {
                if (user.Avatar != null)
                {
                    await _fileService.DeleteFile(user.Avatar, ImageConstants.AVATARS_PATH);
                }
                user.Avatar = await _fileService.SaveFile(doctorDetailsDto.Avatar, ImageConstants.AVATARS_PATH);
            }

            var updatedDoctor = await _doctorRepos.Update(user);
            var doctorDto = _mapper.Map<PatientDetailsDto>(updatedDoctor);
            return Ok(doctorDto);
        }

        public async Task<IActionResult> UpdatePatientProfileAsync([FromForm] PatientUpdateDto patientDetailsDto)
        {
            //var patientModel = _mapper.Map<Patient>(patientDetailsDto);
            var user = _patientRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.AccountId == patientDetailsDto.AccountId)
                                .First();
            if (user == null)
                return Unauthorized();
            user.FullName = patientDetailsDto.FullName;
            user.User.Email = patientDetailsDto.Email;
            user.Languages = patientDetailsDto.Languages;
            user.Phone = patientDetailsDto.Phone;
            user.PostalCode = patientDetailsDto.PostalCode;
            user.Gender = patientDetailsDto.Gender;
            user.DateOfBirth = patientDetailsDto.DateOfBirth;
            user.Address = patientDetailsDto.Address;

            if (patientDetailsDto.Avatar != null)
            {
                if(user.Avatar != null)
                {
                    await _fileService.DeleteFile(user.Avatar, ImageConstants.AVATARS_PATH);
                }
                user.Avatar = await _fileService.SaveFile(patientDetailsDto.Avatar, ImageConstants.AVATARS_PATH);
            }

            var updatedPatient = await _patientRepos.Update(user);
            var patientDto = _mapper.Map<PatientDetailsDto>(updatedPatient);
            return Ok(patientDto);
        }

       

        public async Task<IActionResult> UpdatePharmacyProfileAsync([FromForm]PharmacyUpdateDto pharmacyDetailsDto)
        {
            var user = _pharmacyRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.AccountId == pharmacyDetailsDto.AccountId)
                                .First();
            if (user == null)
                return Unauthorized();
            user.PharmacyName = pharmacyDetailsDto.PharmacyName;
            user.Phone = pharmacyDetailsDto.Phone;
            user.Address = pharmacyDetailsDto.Address;
            if (pharmacyDetailsDto.Avatar != null)
            {
                if (user.Avatar != null)
                {
                    await _fileService.DeleteFile(user.Avatar, ImageConstants.AVATARS_PATH);
                }
                user.Avatar = await _fileService.SaveFile(pharmacyDetailsDto.Avatar, ImageConstants.AVATARS_PATH);
            }

            var updatedPharmacy = await _pharmacyRepos.Update(user);
            var pharmacyDto = _mapper.Map<PharmacyDetailsDto>(updatedPharmacy);
            return Ok(pharmacyDto);
        }
    }
}
