using AutoMapper;
using Business.IServices;
using Contract.DTOs.MailService;
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
        private readonly ISendMailService _sendMailService;

        public MedicalRecordService(ApplicationDbContext db, IMapper mapper,ISendMailService sendMailService)
        {
            _db = db;
            _mapper = mapper;
            _sendMailService = sendMailService;
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

        public async Task<bool> SendMedicalRecord(int patientId,string medicalContent)
        {
            var patient = await _db.Patients.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == patientId);
            if(patient != null)
            {
                var mailContent = new MailContent()
                {
                    To = patient.User.Email,
                    Subject = $"Medical record of {patient.FullName}",
                    Body = $"Hi {patient.FullName}, we send your medical records" +
                        medicalContent +
                        @"
                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='http://fhc.eastasia.cloudapp.azure.com/'>http://fhc.eastasia.cloudapp.azure.com/</a></span> <br>
                        </p>"
                };
                await _sendMailService.SendMail(mailContent);
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
            var medicalRecords = await _db.MedicalRecords.Where(a => a.Doctor.AccountId == accountId || a.Patient.AccountId == accountId)
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
            var medicalRecordInDb = await _db.MedicalRecords.Where(m => m.Id == id)
                                                            .Include(p => p.Patient)
                                                            .Include(d => d.Doctor)
                                                            .FirstOrDefaultAsync();
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
