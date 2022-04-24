using AutoMapper;
using Business.IServices;
using Contract.DTOs.MedicalRecordService;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public MedicalRecordService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        private bool CheckPatientExists(int patientId)
        {
            var patient = _db.Patients.Where(p => p.Id == patientId).FirstOrDefault();
            if(patient != null)
            {
                return true;
            }
            return false;
        }  

        public async Task<MedicalRecordDto> CreateMedicalRecord(AddUpdateMedicalRecordDto AddmedicalRecordDto)
        {
            if (CheckPatientExists(AddmedicalRecordDto.PatientId))
            {
                var medicalRecordModel = _mapper.Map<MedicalRecord>(AddmedicalRecordDto);
                await _db.MedicalRecords.AddAsync(medicalRecordModel);
                await _db.SaveChangesAsync();
                var medicalRecordDetails = await _db.MedicalRecords.Where(m => m.Id == medicalRecordModel.Id)
                                                                    .Include(p => p.Patient)
                                                                    .Include(d => d.Doctor)
                                                                    .FirstOrDefaultAsync();
                var medicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecordDetails);
                return medicalRecordDto;
            }
            return null;
        }

        public async Task<MedicalRecordDto> DeleteMedicalRecord(int id)
        {
            var medicalRecordInDb = await _db.MedicalRecords.FirstOrDefaultAsync(m => m.Id == id);
            if(medicalRecordInDb != null)
            {
                _db.MedicalRecords.Remove(medicalRecordInDb);
                await _db.SaveChangesAsync();
                var medicalDto = _mapper.Map<MedicalRecordDto>(medicalRecordInDb);
                return medicalDto;
            }
            return null;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByDoctor(string accountId)
        {
            var medicalRecords = await _db.MedicalRecords.Where(a => a.Doctor.AccountId == accountId)
                                                         .Include(p => p.Patient)
                                                         .Include(d => d.Doctor)
                                                         .ToListAsync();
            if(medicalRecords.Count > 0)
            {
                var medicalRecordsDto = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
                return medicalRecordsDto;
            }
            return null;                                             
        }

        public async Task<MedicalRecordDto> UpdateMedicalRecord(int id, AddUpdateMedicalRecordDto medicalRecordDto)
        {
            var medicalRecordInDb = await _db.MedicalRecords.Where(m => m.Id == id).FirstOrDefaultAsync();
            if(medicalRecordInDb != null)
            {
                medicalRecordInDb = _mapper.Map<AddUpdateMedicalRecordDto, MedicalRecord>(medicalRecordDto, medicalRecordInDb);
                _db.MedicalRecords.Update(medicalRecordInDb);
                await _db.SaveChangesAsync();
                var medicalDto = _mapper.Map<MedicalRecordDto>(medicalRecordInDb);
                return medicalDto;
            }
            return null;
        }
    }
}
