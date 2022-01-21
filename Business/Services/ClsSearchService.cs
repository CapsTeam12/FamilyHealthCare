using AutoMapper;
using Business.IServices;
using Contract.Constants;
using Contract.DTOs.SearchService;
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
    public class ClsSearchService : ControllerBase, ISearchService
    {
        private readonly IBaseRepository<Medicine> _medicineRepos;
        private readonly IBaseRepository<Doctor> _doctorRepos;
        private readonly IBaseRepository<Pharmacy> _pharmacyRepos;
        private readonly IMapper _mapper;
        public ClsSearchService(IBaseRepository<Medicine> medicineRepos, IMapper mapper, 
                                IBaseRepository<Doctor> doctorRepos, 
                                IBaseRepository<Pharmacy> pharmacyRepos)
        {
            _medicineRepos = medicineRepos;
            _doctorRepos = doctorRepos;
            _pharmacyRepos = pharmacyRepos;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetDetailsSearchPharmacyAsync(int id)
        {
            var pharmacy = await _pharmacyRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.Id.Equals(id))
                                .FirstOrDefaultAsync();
            var pharmacyDtos = _mapper.Map<SearchPharmacyDto>(pharmacy);
            return Ok(pharmacyDtos);
        }

        public async Task<IActionResult> GetDetailsSearchMedicineAsync(int id)
        {
            var medicine = await _medicineRepos
                                .Entities
                                .Include(a => a.MedicineClass)
                                .Where(a => a.Id.Equals(id))
                                .FirstOrDefaultAsync();
            var medicineDtos = _mapper.Map<SearchMedicineDto>(medicine);
            return Ok(medicineDtos);
        }


        public async Task<IActionResult> GetDetailsSearchDoctorAsync(int id)
        {
            var doctor = await _doctorRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.Id.Equals(id))
                                .FirstOrDefaultAsync();
            var doctorDtos = _mapper.Map<SearchDoctorDto>(doctor);
            return Ok(doctorDtos);
        }

        public async Task<IActionResult> GetSearchDoctorResultAsync(string search)
        {
            var doctor = await _doctorRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.User.UserName.ToLower()
                                .Contains(search.ToLower()))
                                .ToListAsync();
            if (doctor.Count == 0)
            {
                return Ok(ErrorMessage.SearchMessage.NullResult);
            }
            var doctorDtos = _mapper.Map<IEnumerable<SearchDoctorDto>>(doctor);
            return Ok(doctorDtos);
        }

        public async Task<IEnumerable<SearchMedicineDto>> GetSearchMedicineResultAsync(SearchCategoryDto searchCategoryDto)
        {
            var medicine = await _medicineRepos
                                .Entities
                                .Include(a => a.MedicineClass)
                                .Where(a => a.MedicineName.ToLower().Contains(searchCategoryDto.Search.ToLower())
                                         || a.Description.ToLower().Contains(searchCategoryDto.Search.ToLower()))
                                .ToListAsync();
            var medicineDtos = _mapper.Map<IEnumerable<SearchMedicineDto>>(medicine);
            if(searchCategoryDto.FilterCates != null)
            {
                medicineDtos = medicineDtos
                    .Where(m => searchCategoryDto.FilterCates.Contains(m.ClassificationName));
            }
            return medicineDtos;
        }

        public async Task<IActionResult> GetSearchPharmacyResultAsync(string search)
        {
            var pharmacy = await _doctorRepos
                                .Entities
                                .Include(a => a.User)
                                .Where(a => a.User.UserName.ToLower()
                                .Contains(search.ToLower()))
                                .ToListAsync();
            if (pharmacy.Count == 0)
            {
                return Ok(ErrorMessage.SearchMessage.NullResult);
            }
            var pharmacyDtos = _mapper.Map<IEnumerable<SearchDoctorDto>>(pharmacy);
            return Ok(pharmacyDtos);
        }

    }
}
