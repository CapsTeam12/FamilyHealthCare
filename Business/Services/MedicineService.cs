using AutoMapper;
using Business.IServices;
using Contract.DTOs.MedicineService;
using Data;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public MedicineService(ApplicationDbContext db, IMapper mapper, IFileService fileService)
        {
            _db = db;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<MedicineDto> CreateMedicine(AddUpdateMedicineDto medicineDto)
        {
            var medicineModel = _mapper.Map<Medicine>(medicineDto);
            if (medicineDto.Images != null)
            {
                medicineModel.Images = await _fileService.SaveFile(medicineDto.Images, ImageConstants.MEDICINES_PATH);
            }
            await _db.Medicines.AddAsync(medicineModel);
            await _db.SaveChangesAsync();
            var medicineDetail = await _db.Medicines.Where(m => m.Id == medicineModel.Id)
                                                    .Include(c => c.MedicineClass)
                                                    .Include(p => p.Pharmacy)
                                                    .FirstOrDefaultAsync();
            var medicineDetailDto = _mapper.Map<MedicineDto>(medicineDetail);
            return medicineDetailDto;
        }

        public async Task<MedicineDto> UpdateMedicine(int id,AddUpdateMedicineDto medicineDto)
        {
            var medicineInDb = await _db.Medicines
                                    .Where(m => m.Id == id)
                                    .FirstOrDefaultAsync();
            //medicineInDb = _mapper.Map<Medicine>(medicineDto); // Mapping but create new instance 
            medicineInDb = _mapper.Map(medicineDto, medicineInDb); // Mapping but not create new instance
            if (medicineDto.Images != null)
            {
                if (medicineInDb.Images != null)
                {
                    await _fileService.DeleteFile(medicineInDb.Images, ImageConstants.MEDICINES_PATH);
                }
                medicineInDb.Images = await _fileService.SaveFile(medicineDto.Images, ImageConstants.MEDICINES_PATH);
            }
            _db.Medicines.Update(medicineInDb);
            await _db.SaveChangesAsync();
            var medicineDetail = await _db.Medicines.Where(m => m.Id == id)   
                                                    .Include(c => c.MedicineClass)
                                                    .Include(p => p.Pharmacy)
                                                    .FirstOrDefaultAsync();
            var medicineDetailDto = _mapper.Map<MedicineDto>(medicineDetail);
            return medicineDetailDto;
        }

        public async Task<MedicineDto> DeleteMedicine(int id)
        {
            var medicineInDb = await _db.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            if (medicineInDb != null)
            {
                _db.Medicines.Remove(medicineInDb);
                if (medicineInDb.Images != null)
                {
                    await _fileService.DeleteFile(medicineInDb.Images, ImageConstants.MEDICINES_PATH);
                }
                await _db.SaveChangesAsync();
                var medicineDto = _mapper.Map<MedicineDto>(medicineInDb);
                return medicineDto;
            }
            return null;
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicines()
        {
            var medicines = await _db.Medicines
                        .Include(c => c.MedicineClass)
                        .Include(p => p.Pharmacy)
                        .ToListAsync();
            var medicinesDto = _mapper.Map<IEnumerable<MedicineDto>>(medicines);
            return medicinesDto;
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicinesByPharmacyId(string id)
        {
            var medicines = await _db.Medicines
                           .Where(m => m.Pharmacy.AccountId == id)
                           .Include(c => c.MedicineClass)
                           .Include(p => p.Pharmacy)
                           .ToListAsync();
            var medicinesDto = _mapper.Map<IEnumerable<MedicineDto>>(medicines);
            return medicinesDto;
        }

        public async Task<MedicineDto> ReturnMedicine(int id)
        {
            var medicine = await _db.Medicines.Where(m => m.Id == id)
                                              .Include(c => c.MedicineClass)
                                              .Include(p => p.Pharmacy)
                                              .FirstOrDefaultAsync();
            if(medicine != null)
            {
                medicine.Status = 3;
                _db.Medicines.Update(medicine);
                await _db.SaveChangesAsync();
                var medicineDto = _mapper.Map<MedicineDto>(medicine);
                return medicineDto;
            }
            return null;
        }
    }
}
