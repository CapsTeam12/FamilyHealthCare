using AutoMapper;
using Business.IServices;
using Contract.DTOs.ManagementService;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClsManagementService : ControllerBase, IManagementService
    {
        private readonly IBaseRepository<Doctor> _doctorRepos;
        private readonly IBaseRepository<Pharmacy> _pharmacyRepos;
        private readonly IBaseRepository<MedicineClassification> _cateRepos;
        private readonly IBaseRepository<User> _patientRepos;
        private readonly IBaseRepository<Specialities> _specializedRepos;
        private readonly IMapper _mapper;
        
        public ClsManagementService(IMapper mapper,
                                    IBaseRepository<MedicineClassification> cateRepos,
                                    IBaseRepository<Doctor> doctorRepos,
                                    IBaseRepository<Pharmacy> pharmacyRepos,
                                    IBaseRepository<User> patientRepos,
                                    IBaseRepository<Specialities> specializedRepos)
        {
            _cateRepos = cateRepos;
            _doctorRepos = doctorRepos;
            _pharmacyRepos = pharmacyRepos;
            _patientRepos = patientRepos;
            _specializedRepos = specializedRepos;
            _mapper = mapper;
        }

        public async Task<List<CategoriesDetailsDto>> GetCategoriestAsync()
        {
            var categories = await _cateRepos
                              .Entities
                              .OrderByDescending(a => a.ClassificationName)
                              .ToListAsync();
            var categoriesDtos = _mapper.Map<List<CategoriesDetailsDto>>(categories);
            return categoriesDtos;
        }

        public async Task<List<DoctorDetailsDto>> GetDoctorsAsync()
        {
            var doctors = await _doctorRepos
                               .Entities
                               .Include(a => a.User)
                               .OrderByDescending(a => a.User.FullName)
                               .ToListAsync();
            var doctorDtos = _mapper.Map<List<DoctorDetailsDto>>(doctors);
            return doctorDtos;
        }

        public async Task<IActionResult> GetPatientsAsync()
        {
            var patients = await _doctorRepos
                              .Entities
                              .Include(a => a.User)
                              .OrderByDescending(a => a.User.FullName)
                              .ToListAsync();
            var patientsDtos = _mapper.Map<List<PatientDetailsDto>>(patients);
            return Ok(patientsDtos);
        }

        public async Task<IActionResult> GetPharmaciesAsync()
        {
            var pharmacies = await _pharmacyRepos
                              .Entities
                              .Include(a => a.User)
                              .OrderByDescending(a => a.User.FullName)
                              .ToListAsync();
            var pharmaciesDtos = _mapper.Map<List<PharmacyDetailsDto>>(pharmacies);
            return Ok(pharmaciesDtos);
        }

        public async Task<IActionResult> GetSpecialitiesAsync()
        {
            var specialities = await _specializedRepos
                              .Entities
                              .OrderByDescending(a => a.SpecializedName)
                              .ToListAsync();
            var specialitiesDtos = _mapper.Map<List<SpecialitiesDetailsDto>>(specialities);
            return Ok(specialitiesDtos);
        }
    }
}
