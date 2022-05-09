using AutoMapper;
using Business.IServices;
using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicineService;
using Contract.DTOs.PrescriptionService;
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
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public PrescriptionService(ApplicationDbContext db, IMapper mapper, IFileService fileService)
        {
            _db = db;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<PrescriptionDto> AddPrescriptionByDoctor(AddUpdatePrescriptionDto AddPrescriptionDto)
        {
            var prescription = _mapper.Map<Prescription>(AddPrescriptionDto);
            if (AddPrescriptionDto.Signature != null)
            {
                prescription.Signature = await _fileService.SaveFile(AddPrescriptionDto.Signature, ImageConstants.SIGNATURES_PATH);
            }
            await _db.Prescriptions.AddAsync(prescription);
            await _db.SaveChangesAsync();
            var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);
            prescriptionDto.PrescriptionDetailsDtos.AddRange(AddPrescriptionDto.PrescriptionDetailsDtos);
            foreach (var pres in AddPrescriptionDto.PrescriptionDetailsDtos)
            {
                for (int i = 0; i < pres.Time.Length; i++)
                {
                    pres.PrescriptionId = prescription.Id;
                    var prescriptionDetails = new PrescriptionDetails();
                    prescriptionDetails.PrescriptionId = pres.PrescriptionId;
                    prescriptionDetails.MedicineName = pres.MedicineName;
                    prescriptionDetails.Quantity = pres.Quantity;
                    prescriptionDetails.Days = pres.Days;
                    prescriptionDetails.Time = pres.Time[i];
                    await _db.PrescriptionDetails.AddAsync(prescriptionDetails);
                }
            }
            await _db.SaveChangesAsync();
            return prescriptionDto;
        }

        public async Task<PrescriptionDto> AddPrescriptionByPharmacy(AddUpdatePrescriptionPharmacyDto AddPrescriptionDto)
        {
            var prescription = _mapper.Map<Prescription>(AddPrescriptionDto);
            var pharmacy = await _db.Pharmacies.FirstOrDefaultAsync(p => p.Id == prescription.PharmacyId);
            var pharmacyDto = _mapper.Map<PharmacyDto>(pharmacy);
            if (AddPrescriptionDto.Signature != null)
            {
                prescription.Signature = await _fileService.SaveFile(AddPrescriptionDto.Signature, ImageConstants.SIGNATURES_PATH);
            }
            await _db.Prescriptions.AddAsync(prescription);
            await _db.SaveChangesAsync();
            var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);
            prescriptionDto.Pharmacy = pharmacyDto;
            prescriptionDto.PrescriptionDetailsDtos.AddRange(AddPrescriptionDto.PrescriptionDetailsDtos);
            foreach (var pres in AddPrescriptionDto.PrescriptionDetailsDtos)
            {
                for (int i = 0; i < pres.Time.Length; i++)
                {
                    pres.PrescriptionId = prescription.Id;
                    var prescriptionDetails = new PrescriptionDetails();
                    prescriptionDetails.PrescriptionId = pres.PrescriptionId;
                    prescriptionDetails.MedicineName = pres.MedicineName;
                    prescriptionDetails.Quantity = pres.Quantity;
                    prescriptionDetails.Days = pres.Days;
                    prescriptionDetails.Time = pres.Time[i];
                    await _db.PrescriptionDetails.AddAsync(prescriptionDetails);
                }
            }
            await _db.SaveChangesAsync();
            return prescriptionDto;
        }

        public async Task<PrescriptionDto> DeletePrescription(int id)
        {
            var prescription = await _db.Prescriptions.Where(p => p.Id == id || p.MedicalRecordId == id)
                                                      .FirstOrDefaultAsync();
            if (prescription != null)
            {
                _db.Prescriptions.Remove(prescription);
                if (prescription.Signature != null)
                {
                    await _fileService.DeleteFile(prescription.Signature, ImageConstants.SIGNATURES_PATH);
                }
                await _db.SaveChangesAsync();
                var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);
                return prescriptionDto;
            }
            return null;
        }

        public async Task<PrescriptionDto> GetPrescriptionDetails(int id)
        {
            var prescriptionDetails = await _db.PrescriptionDetails.Where(p => p.PrescriptionId == id || p.Prescription.MedicalRecordId == id)
                                                                   .Include(p => p.Prescription)
                                                                   .ToListAsync();
            var medicineInPresciptionDetails = prescriptionDetails.Select(m => m.MedicineName).Distinct();
            if (prescriptionDetails.Count > 0)
            {
                var prescriptionsDetailsDto = new List<PrescriptionDetailsDto>();
                foreach (var medicineName in medicineInPresciptionDetails)
                {
                    var PrescriptionDetailsByMedicine = await _db.PrescriptionDetails.Where(p => (p.PrescriptionId == id || p.Prescription.MedicalRecordId == id) && p.MedicineName == medicineName)
                                                                                 .ToListAsync();
                    var prescriptionDetailsDto = new PrescriptionDetailsDto();
                    prescriptionDetailsDto.PrescriptionId = PrescriptionDetailsByMedicine.Select(p => p.PrescriptionId).First();
                    prescriptionDetailsDto.MedicineName = PrescriptionDetailsByMedicine.Select(m => m.MedicineName).First();
                    prescriptionDetailsDto.Quantity = PrescriptionDetailsByMedicine.Select(q => q.Quantity).First();
                    prescriptionDetailsDto.Days = PrescriptionDetailsByMedicine.Select(d => d.Days).First();

                    prescriptionDetailsDto.Time = new int[PrescriptionDetailsByMedicine.Count];
                    for (int i = 0; i < PrescriptionDetailsByMedicine.Count; i++)
                    {
                        prescriptionDetailsDto.Time[i] = PrescriptionDetailsByMedicine[i].Time;
                    }
                    prescriptionsDetailsDto.Add(prescriptionDetailsDto);
                }
                var prescriptionDto = new PrescriptionDto();
                prescriptionDto.PrescriptionDetailsDtos = prescriptionsDetailsDto;
                var prescription = await _db.Prescriptions.Where(p => p.Id == id || p.MedicalRecordId == id)
                                                          .Include(p => p.Patient)
                                                          .Include(d => d.Doctor)
                                                          .Include(p => p.Pharmacy)
                                                          .Include(m => m.MedicalRecord)
                                                          .FirstOrDefaultAsync();
                prescriptionDto = _mapper.Map<Prescription, PrescriptionDto>(prescription, prescriptionDto);
                return prescriptionDto;
            }
            return null;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsDoctor(string accountId)
        {
            var prescriptions = await _db.Prescriptions.Where(a => a.Doctor.AccountId == accountId)
                                                       .Include(d => d.Doctor)
                                                       .Include(s => s.Doctor.Specialized)
                                                       .ToListAsync();
            var prescriptionsDto = _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
            return prescriptionsDto;
        }


        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsPharmacy(string accountId)
        {
            var prescriptions = await _db.Prescriptions.Where(a => a.Pharmacy.AccountId == accountId)
                                                       .Include(p => p.Pharmacy)
                                                       .ToListAsync();
            var prescriptionsDto = _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
            return prescriptionsDto;
        }

        public async Task<PrescriptionDto> UpdatePrescriptionByDoctor(int id, AddUpdatePrescriptionDto AddPrescriptionDto)
        {
            var prescriptionInDb = await _db.Prescriptions.Where(p => p.Id == id || p.MedicalRecordId == id).FirstOrDefaultAsync();
            if (prescriptionInDb != null)
            {
                prescriptionInDb = _mapper.Map<AddUpdatePrescriptionDto, Prescription>(AddPrescriptionDto, prescriptionInDb);
                if (AddPrescriptionDto.Signature != null)
                {
                    if (prescriptionInDb.Signature != null)
                    {
                        await _fileService.DeleteFile(prescriptionInDb.Signature, ImageConstants.SIGNATURES_PATH);
                    }
                    prescriptionInDb.Signature = await _fileService.SaveFile(AddPrescriptionDto.Signature, ImageConstants.SIGNATURES_PATH);
                }
                _db.Prescriptions.Update(prescriptionInDb);
                var prescriptionDetails = await _db.PrescriptionDetails.Where(p => p.PrescriptionId == id || p.Prescription.MedicalRecordId == id).ToListAsync();
                if (prescriptionDetails != null && prescriptionDetails.Count > 0)
                {
                    _db.PrescriptionDetails.RemoveRange(prescriptionDetails);
                }
                foreach (var pres in AddPrescriptionDto.PrescriptionDetailsDtos)
                {
                    pres.PrescriptionId = prescriptionInDb.Id;
                    for (int i = 0; i < pres.Time.Length; i++)
                    {
                        var prescriptionDetailsAddMore = new PrescriptionDetails();
                        prescriptionDetailsAddMore.PrescriptionId = pres.PrescriptionId;
                        prescriptionDetailsAddMore.MedicineName = pres.MedicineName;
                        prescriptionDetailsAddMore.Quantity = pres.Quantity;
                        prescriptionDetailsAddMore.Days = pres.Days;
                        prescriptionDetailsAddMore.Time = pres.Time[i];
                        await _db.PrescriptionDetails.AddAsync(prescriptionDetailsAddMore);
                    }
                }
                await _db.SaveChangesAsync();
                var prescriptionDto = _mapper.Map<PrescriptionDto>(prescriptionInDb);
                prescriptionDto.PrescriptionDetailsDtos.AddRange(AddPrescriptionDto.PrescriptionDetailsDtos);
                return prescriptionDto;
            }
            return null;
        }

        public async Task<PrescriptionDto> UpdatePrescriptionByPharmacy(int id, AddUpdatePrescriptionPharmacyDto AddPrescriptionDto)
        {
            var prescriptionInDb = await _db.Prescriptions.Where(p => p.Id == id || p.MedicalRecordId == id).FirstOrDefaultAsync();
            var pharmacy = await _db.Pharmacies.FirstOrDefaultAsync(p => p.Id == AddPrescriptionDto.PharmacyId);
            var pharmacyDto = _mapper.Map<PharmacyDto>(pharmacy);
            if (prescriptionInDb != null)
            {
                prescriptionInDb = _mapper.Map<AddUpdatePrescriptionPharmacyDto, Prescription>(AddPrescriptionDto, prescriptionInDb);
                if (AddPrescriptionDto.Signature != null)
                {
                    if (prescriptionInDb.Signature != null)
                    {
                        await _fileService.DeleteFile(prescriptionInDb.Signature, ImageConstants.SIGNATURES_PATH);
                    }
                    prescriptionInDb.Signature = await _fileService.SaveFile(AddPrescriptionDto.Signature, ImageConstants.SIGNATURES_PATH);
                }
                _db.Prescriptions.Update(prescriptionInDb);
                var prescriptionDetails = await _db.PrescriptionDetails.Where(p => p.PrescriptionId == id || p.Prescription.MedicalRecordId == id).ToListAsync();
                if (prescriptionDetails != null && prescriptionDetails.Count > 0)
                {
                    _db.PrescriptionDetails.RemoveRange(prescriptionDetails);
                }
                foreach (var pres in AddPrescriptionDto.PrescriptionDetailsDtos)
                {
                    pres.PrescriptionId = prescriptionInDb.Id;
                    for (int i = 0; i < pres.Time.Length; i++)
                    {
                        var prescriptionDetailsAddMore = new PrescriptionDetails();
                        prescriptionDetailsAddMore.PrescriptionId = pres.PrescriptionId;
                        prescriptionDetailsAddMore.MedicineName = pres.MedicineName;
                        prescriptionDetailsAddMore.Quantity = pres.Quantity;
                        prescriptionDetailsAddMore.Days = pres.Days;
                        prescriptionDetailsAddMore.Time = pres.Time[i];
                        await _db.PrescriptionDetails.AddAsync(prescriptionDetailsAddMore);
                    }
                }
                await _db.SaveChangesAsync();
                var prescriptionDto = _mapper.Map<PrescriptionDto>(prescriptionInDb);
                prescriptionDto.Pharmacy = pharmacyDto;
                prescriptionDto.PrescriptionDetailsDtos.AddRange(AddPrescriptionDto.PrescriptionDetailsDtos);
                return prescriptionDto;
            }
            return null;
        }
    }
}
