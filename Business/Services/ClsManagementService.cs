using AutoMapper;
using Business.IServices;
using Contract.Constants;
using Contract.DTOs.ManagementService;
using Contract.DTOs.NotificationServiceDtos;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNet.Identity;
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
        private readonly IBaseRepository<Patient> _patientRepos;
        private readonly IBaseRepository<Specialities> _specializedRepos;
        private readonly IBaseRepository<User> _userRepos;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ClsManagementService(IMapper mapper,
                                    IBaseRepository<MedicineClassification> cateRepos,
                                    IBaseRepository<Doctor> doctorRepos,
                                    IBaseRepository<Pharmacy> pharmacyRepos,
                                    IBaseRepository<Patient> patientRepos,
                                    IBaseRepository<Specialities> specializedRepos,
                                    IBaseRepository<User> userRepos,
			            IFileService fileService)
        {
            _cateRepos = cateRepos;
            _doctorRepos = doctorRepos;
            _pharmacyRepos = pharmacyRepos;
            _patientRepos = patientRepos;
            _specializedRepos = specializedRepos;
            _userRepos = userRepos;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<List<CategoriesDetailsDto>> GetCategoriesAsync()
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
                               .Where(d => d.AccountId != null)
                               .Include(a => a.User)
                               .Include(s => s.Specialized)
                               .OrderByDescending(a => a.FullName)
                               .ToListAsync();
            var doctorDtos = _mapper.Map<List<DoctorDetailsDto>>(doctors);
            return doctorDtos;
        }

        public async Task<List<PatientDetailsDto>> GetPatientsAsync()
        {
            var patients = await _patientRepos
                              .Entities
                              .Include(a => a.User)
                              .OrderByDescending(a => a.FullName)
                              .ToListAsync();
            var patientsDtos = _mapper.Map<List<PatientDetailsDto>>(patients);
            return patientsDtos;
        }

        public async Task<List<PharmacyDetailsDto>> GetPharmaciesAsync()
        {
            var pharmacies = await _pharmacyRepos
                              .Entities
                              .Where(p => p.AccountId != null)
                              .Include(a => a.User)
                              .OrderByDescending(a => a.PharmacyName)
                              .ToListAsync();
            var pharmaciesDtos = _mapper.Map<List<PharmacyDetailsDto>>(pharmacies);
            return pharmaciesDtos;
        }

        public async Task<List<SpecialitiesDetailsDto>> GetSpecialitiesAsync()
        {
            var specialities = await _specializedRepos
                              .Entities
                              .OrderByDescending(a => a.SpecializedName)
                              .ToListAsync();
            var specialitiesDtos = _mapper.Map<List<SpecialitiesDetailsDto>>(specialities);
            return specialitiesDtos;
        }

        public async Task<SpecialitiesDetailsDto> UpdateSpecialities(SpecialitiesUpdateDto specialitiesUpdateDto)
        {
            var specialist = await _specializedRepos.Entities.Where(s => s.Id == specialitiesUpdateDto.Id).FirstAsync();
            specialist.SpecializedName = specialitiesUpdateDto.SpecializedName;
            if (specialitiesUpdateDto.Image != null)
            {
                if (specialist.Image != null)
                {
                    await _fileService.DeleteFile(specialist.Image, ImageConstants.SPECIALIST_PATH);
                }
                specialist.Image = await _fileService.SaveFile(specialitiesUpdateDto.Image, ImageConstants.SPECIALIST_PATH);
            }
            var updateSpecialist = await _specializedRepos.Update(specialist);
            var specialitiesDto = _mapper.Map<SpecialitiesDetailsDto>(updateSpecialist);
            return specialitiesDto;
        }

        public Task<CategoriesDetailsDto> GetCategoryDetailsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<DoctorDetailsDto> GetDoctorDetailsAsync(string id)
        {
            var doctor = await _doctorRepos
                               .Entities
                               .Include(a => a.User)
                               .Include(s => s.Specialized)
                               .Where(x => x.AccountId == id)
                               .FirstOrDefaultAsync();
            var doctorDto = _mapper.Map<DoctorDetailsDto>(doctor);
            return doctorDto;
        }

        public async Task<PatientDetailsDto> GetPatientDetailsAsync(string id)
        {
            var patient = await _patientRepos
                              .Entities
                              .Include(a => a.User)
                              .Where(x => x.AccountId == id)
                              .FirstOrDefaultAsync();
            var patientsDto = _mapper.Map<PatientDetailsDto>(patient);
            if(patient != null)
            {
                patientsDto.Email = patient.User.Email;
            }
            return patientsDto;
        }

        public async Task<PharmacyDetailsDto> GetPharmacyDetailsAsync(string id)
        {
            var pharmacy = await _pharmacyRepos
                                    .Entities
                                    .Include(a => a.User)
                                    .Where(x => x.AccountId == id)
                                    .FirstOrDefaultAsync();
            var pharmacyDto = _mapper.Map<PharmacyDetailsDto>(pharmacy);
            return pharmacyDto;
        }

        public async Task<SpecialitiesDetailsDto> AdddSpecializedAsync(SpecialitiesUpdateDto specialitiesDetailsDto)
        {
            var specialized = _mapper.Map<Specialities>(specialitiesDetailsDto);
            if(specialitiesDetailsDto.Image != null)
            {
                specialized.Image = await _fileService.SaveFile(specialitiesDetailsDto.Image, ImageConstants.SPECIALIST_PATH);
            }
            var newSpecialized = await _specializedRepos.Create(specialized);
            var specializedDto = _mapper.Map<SpecialitiesDetailsDto>(newSpecialized);
            return specializedDto;
        }

        public async Task<CategoriesDetailsDto> AddCategoryAsync(CategoriesDetailsDto categoriesDetailsDto)
        {
            var category = _mapper.Map<MedicineClassification>(categoriesDetailsDto);
            var newCategory = await _cateRepos.Create(category);
            var categorydDto = _mapper.Map<CategoriesDetailsDto>(newCategory);
            return categorydDto;
        }

        public async Task<SpecialitiesDetailsDto> UpdateSpecializedAsync(int id, SpecialitiesUpdateDto specialitiesUpdateDto)
        {
            var specialist = await _specializedRepos.Entities.Where(s => s.Id == id).FirstAsync();
            specialist.SpecializedName = specialitiesUpdateDto.SpecializedName;
            if (specialitiesUpdateDto.Image != null)
            {
                if (specialist.Image != null)
                {
                    await _fileService.DeleteFile(specialist.Image, ImageConstants.SPECIALIST_PATH);
                }
                specialist.Image = await _fileService.SaveFile(specialitiesUpdateDto.Image, ImageConstants.SPECIALIST_PATH);
            }
            var updateSpecialist = await _specializedRepos.Update(specialist);
            var specialitiesDto = _mapper.Map<SpecialitiesDetailsDto>(updateSpecialist);
            return specialitiesDto;
        }

        public async  Task<CategoriesDetailsDto> UpdateCategoryAsync(int id, CategoriesDetailsDto categoriesDetailsDto)
        {
            var category = await _cateRepos
                                    .Entities
                                    .Where(s => s.Id == id)
                                    .FirstOrDefaultAsync();
            category.ClassificationName = categoriesDetailsDto.CateName;
            var updateCategory = await _cateRepos.Update(category);
            var categorydDto = _mapper.Map<CategoriesDetailsDto>(updateCategory);
            return categorydDto;
        }

        public async  Task<SpecialitiesDetailsDto> DeleteSpecializedAsync(int id)
        {
            var specialized = await _specializedRepos
                                    .Entities
                                    .Where(s => s.Id == id)
                                    .FirstOrDefaultAsync();
            if(specialized.Image != null)
            {
                await _fileService.DeleteFile(specialized.Image, ImageConstants.SPECIALIST_PATH);
            }
            var deleteSpecialized = await _specializedRepos.Delete(specialized);           
            var specializedDto = _mapper.Map<SpecialitiesDetailsDto>(deleteSpecialized);
            return specializedDto;
        }

        public async Task<CategoriesDetailsDto> DeleteCategoryAsync(int id)
        {
            var category = await _cateRepos
                                    .Entities
                                    .Where(s => s.Id == id)
                                    .FirstOrDefaultAsync();
            var deleteCategory = await _cateRepos.Delete(category);
            var categorydDto = _mapper.Map<CategoriesDetailsDto>(deleteCategory);
            return categorydDto;
        }   

        public async Task<User> ActivePatientsAsync(string accountId)
        {
            var user = await _userRepos
                                .Entities
                                .Where(a => a.Id == accountId)
                                .FirstOrDefaultAsync();
            user.IsActive = true;
            var activeUser = await _userRepos.Update(user);
            var userDto = _mapper.Map<User>(activeUser);
            return userDto;
        }

        public async Task<User> DeactivatePatientsAsync(string accountId)
        {
            var user = await _userRepos
                                .Entities
                                .Where(a => a.Id == accountId)
                                .FirstOrDefaultAsync();
            user.IsActive = false;
            var activeUser = await _userRepos.Update(user);
            var userDto = _mapper.Map<User>(activeUser);

            var notification = new NotificationCreateDto();

            notification.UserID = accountId;
            notification.Content = NotificationContentTemplate.DeactivateUser;
                                    
            notification.AvatarSender = ComonConstant.LogoFileName;

            Task.Run(() => new NotificationHelper().CallApiCreateNotification(notification));

            return userDto;
        }
    }
}
