using AutoMapper;
using Business.IServices;
using Contract.DTOs.MailService;
using Contract.DTOs.ManagementService;
using Contract.DTOs.PartnerService;
using Data;
using Data.Entities;
using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public static class Extensions
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            List<char> charsToRemove = new List<char>() { '@', '_', ',', '.' };
            foreach (char c in charsToRemove)
            {
                str = str.Replace(c.ToString(), String.Empty);
            }

            return str;
        }
    }

    public class PartnerService : IParnerService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly ISendMailService _sendMail;

        public PartnerService(ApplicationDbContext db, IMapper mapper, IFileService fileService, UserManager<User> userManager, ISendMailService sendMail)
        {
            _db = db;
            _mapper = mapper;
            _fileService = fileService;
            _userManager = userManager;
            _sendMail = sendMail;
        }


        public async Task<DoctorDetailsDto> AcceptDoctorRequest(int doctorId)
        {
            var doctorModel = await _db.Doctors.Where(d => d.AccountId == null).FirstOrDefaultAsync(x => x.Id == doctorId);
            int charPos = doctorModel.Email.IndexOf('@');
            string newString = doctorModel.Email.Substring(0, charPos);
            string userName = newString.RemoveSpecialCharacters();

            Random rd = new Random();
            int randomNumber = rd.Next(100, 999);
            string password = "fhc" + randomNumber;
            var user = new User();
            user.UserName = userName;
            user.Email = doctorModel.Email;
            user.PhoneNumber = doctorModel.Phone;
            while (await _userManager.FindByNameAsync(userName) != null)
            {
                userName = userName + rd.Next(1, 9);
                user.UserName = userName;
            }
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                doctorModel.AccountId = user.Id;
                await _userManager.AddToRoleAsync(user, "Doctor");
                string EmailDoctor = doctorModel.Email;
                string Subject = "Respond to your partner request from FHC";
                string htmlMessage = "<h2>Welcome to Family Health Care,…</h2> <hr>" +
                                       "<p>You have become our partner,…</p>" +
                                       "<p>Use this account to access our service and contact your patient: </p>" +
                                       $"<p>UserName: <b>{user.UserName}</b>, Password: <b>{password}</b> </p>" +
                                       "<p>Access path: <a href='https://abc.com'>https://abc.com</a></p>" +
                                        "<p>Please kindly learn the user guide in the attached file. </p>" +
                                       "<p>For more information please contact us at: <b>fhc.health@gmail.com</b> or hotline: <b>09990909</b> </p> <hr>" +
                                       @"<p>
                                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                                            width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health@gmail.com</b></span><br>
                                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a href='https://abc.com'>https://abc.com</a></span> <br>
                                        </p>";
                string folderPath = Path.Combine(WebHostEnviromentHelper.GetWebRootPath(), "userguide/");
                string path = Path.Combine(folderPath, "mail-reply-request.docx");

                var stream = File.OpenRead(path);
                FormFile Attachment = new FormFile(stream, 0, stream.Length, "Attachments", Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentDisposition = "form-data; name=\"Attachments\"; filename=\"mail-reply-request.docx\"",
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };
                MailContent mailContent = new MailContent()
                {
                    To = EmailDoctor,
                    Subject = Subject,
                    Body = htmlMessage,                   
                };
                if (Attachment != null)
                {
                    mailContent.Attachments = new List<IFormFile>();
                    mailContent.Attachments.Add(Attachment);
                }
                await _sendMail.SendMail(mailContent);
            }
            var doctorDto = _mapper.Map<DoctorDetailsDto>(doctorModel);
            return doctorDto;
        }

        public async Task<PharmacyDetailsDto> AcceptPharmacyRequest(int pharmacyId)
        {
            var pharmacyModel = await _db.Pharmacies.Where(d => d.AccountId == null).FirstOrDefaultAsync(x => x.Id == pharmacyId);
            int charPos = pharmacyModel.Email.IndexOf('@');
            string newString = pharmacyModel.Email.Substring(0, charPos);
            string userName = newString.RemoveSpecialCharacters();

            Random rd = new Random();
            int randomNumber = rd.Next(100, 999);
            string password = "fhc" + randomNumber;
            var user = new User();
            user.UserName = userName;
            user.Email = pharmacyModel.Email;
            user.PhoneNumber = pharmacyModel.Phone;
            while(await _userManager.FindByNameAsync(userName) != null)
            {
                userName = userName + rd.Next(1, 9);
                user.UserName = userName;
            }
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                pharmacyModel.AccountId = user.Id;
                await _userManager.AddToRoleAsync(user, "Pharmacy");
                string EmailPharmacy = pharmacyModel.Email;
                string Subject = "Respond to your partner request from FHC";
                string htmlMessage = "<h2>Welcome to Family Health Care,…</h2> <hr>" +
                                       "<p>You have become our partner,…</p>" +
                                       "<p>Use this account to access our service and contact your patient: </p>" +
                                       $"<p>UserName: <b>{user.UserName}</b>, Password: <b>{password}</b> </p>" +
                                       "<p>Access path: <a href='https://abc.com'>https://abc.com</a></p>" +
                                        "<p>Please kindly learn the user guide in the attached file. </p>" +
                                       "<p>For more information please contact us at: <b>fhc.health@gmail.com</b> or hotline: <b>09990909</b> </p> <hr>" +
                                       @"<p>
                                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                                            width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health@gmail.com</b></span><br>
                                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a href='https://abc.com'>https://abc.com</a></span> <br>
                                        </p>";
                string folderPath = Path.Combine(WebHostEnviromentHelper.GetWebRootPath(), "userguide/");
                string path = Path.Combine(folderPath, "mail-reply-request.docx");

                var stream = File.OpenRead(path);
                FormFile Attachment = new FormFile(stream, 0, stream.Length, "Attachments", Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentDisposition = "form-data; name=\"Attachments\"; filename=\"mail-reply-request.docx\"",
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };
                MailContent mailContent = new MailContent()
                {
                    To = EmailPharmacy,
                    Subject = Subject,
                    Body = htmlMessage,
                };
                if (Attachment != null)
                {
                    mailContent.Attachments = new List<IFormFile>();
                    mailContent.Attachments.Add(Attachment);
                }
                await _sendMail.SendMail(mailContent);
            }
            var pharmacyDto = _mapper.Map<PharmacyDetailsDto>(pharmacyModel);
            return pharmacyDto;
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            var emailOfDoctor = await _db.Doctors.Where(d => d.Email == email).FirstOrDefaultAsync();
            var emailOfPharmacy = await _db.Pharmacies.Where(p => p.Email == email).FirstOrDefaultAsync();
            var emailOfUser = await _db.Patients.Where(p => p.User.Email == email).FirstOrDefaultAsync();
            if(emailOfDoctor != null || emailOfPharmacy != null || emailOfUser != null)
            {
                return false;
            }
            return true;
        }

        public async Task<DoctorDetailsDto> DenyDoctorRequest(int doctorId)
        {
            var doctorReq = await _db.Doctors.Where(d => d.AccountId == null).FirstOrDefaultAsync(d => d.Id == doctorId);
            if(doctorReq != null)
            {
                _db.Doctors.Remove(doctorReq);
                await _db.SaveChangesAsync();
                string EmailDoctor = doctorReq.Email;
                string Subject = "Respond to your partner request from fhc";
                string htmlMessage = $"<h2>Hi, {doctorReq.FullName}</h2>" +
                                     @"<p>We sincerely appreciate you taking the time to interview and offer your request to us. After much consideration, we have decided to deny your request.</p>
                                        <p>I wish you all the best in finding another job suitable for you.</p>
                                        <h3>Kind regards,</h3>
                                        <i>FHC Team</i>
                                        <hr>
                                        <p>
                                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                                            width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health@gmail.com</b></span><br>
                                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a href='https://abc.com'>https://abc.com</a></span> <br>
                                         </p>";

                MailContent mailContent = new MailContent()
                {
                    To = EmailDoctor,
                    Subject = Subject,
                    Body = htmlMessage,
                };
                await _sendMail.SendMail(mailContent);
                var doctorDto = _mapper.Map<DoctorDetailsDto>(doctorReq);
                return doctorDto;
            }
            return null;
        }

        public async Task<PharmacyDetailsDto> DenyPharmacyRequest(int pharmacyId)
        {
            var pharmacyReq = await _db.Pharmacies.Where(d => d.AccountId == null).FirstOrDefaultAsync(d => d.Id == pharmacyId);
            if(pharmacyReq != null)
            {
                _db.Pharmacies.Remove(pharmacyReq);
                await _db.SaveChangesAsync();
                string EmailPharmacy = pharmacyReq.Email;
                string Subject = "Respond to your partner request from fhc";
                string htmlMessage = $"<h2>Hi, {pharmacyReq.PharmacyName}</h2>" +
                                     @"<p>We sincerely appreciate you taking the time to interview and offer your request to us. After much consideration, we have decided to deny your request.</p>
                                   <p>I wish you all the best in finding another job suitable for you.</p>
                                   <h3>Kind regards,</h3>
                                   <i>FHC Team</i>
                                   <hr>
                                   <p>
                                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                                            width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health@gmail.com</b></span><br>
                                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a href='https://abc.com'>https://abc.com</a></span> <br>
                                   </p>";

                MailContent mailContent = new MailContent()
                {
                    To = EmailPharmacy,
                    Subject = Subject,
                    Body = htmlMessage,
                };
                await _sendMail.SendMail(mailContent);
                var pharmacyDto = _mapper.Map<PharmacyDetailsDto>(pharmacyReq);
                return pharmacyDto;
            }
            return null;
        }

        public async Task<DoctorRegisterDto> DoctorRegister(DoctorRegisterDto doctorRegisterDto)
        {
            var doctorModel = _mapper.Map<Doctor>(doctorRegisterDto);
            if (doctorRegisterDto.Avatar != null)
            {
                doctorModel.Avatar = await _fileService.SaveFile(doctorRegisterDto.Avatar, ImageConstants.AVATARS_PATH);
            }
            if (doctorRegisterDto.Certifications != null)
            {
                doctorModel.Certifications = await _fileService.SaveFile(doctorRegisterDto.Certifications, ImageConstants.CERTIFICATION_DOCTORS_PATH);
            }
            await _db.Doctors.AddAsync(doctorModel);
            await _db.SaveChangesAsync();
            foreach (var exp in doctorRegisterDto.Experiences)
            {
                exp.DoctorId = doctorModel.Id;
                var experiencesModel = _mapper.Map<Experience>(exp);
                await _db.Experiences.AddAsync(experiencesModel);
            }
            await _db.SaveChangesAsync();
            var doctorDto = _mapper.Map<DoctorRegisterDto>(doctorRegisterDto);
            return doctorDto;
        }

        public async Task<IEnumerable<DoctorRequestDetailsDto>> DoctorRequestList()
        {
            var doctorReq = await _db.Doctors.Where(d => d.AccountId == null).Include(s => s.Specialized).ToListAsync();
            var doctorReqDto = _mapper.Map<IEnumerable<DoctorRequestDetailsDto>>(doctorReq);
            foreach (var doctor in doctorReqDto)
            {
                var experiences = await _db.Experiences.Where(d => d.DoctorId == doctor.Id).ToListAsync();
                doctor.Experiences = _mapper.Map<List<ExperiencesDto>>(experiences);
            }
            return doctorReqDto;
        }

        public async Task<DoctorDetailsDto> GetDetailsDoctorRequest(int doctorId)
        {
            var doctorModel = await _db.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
            var doctorDto = _mapper.Map<DoctorDetailsDto>(doctorModel);
            return doctorDto;
        }

        public async Task<PharmacyDetailsDto> GetDetailsPharmacyRequest(int pharmacyId)
        {
            var pharmacyModel = await _db.Pharmacies.FirstOrDefaultAsync(p => p.Id == pharmacyId);
            var pharmacyDto = _mapper.Map<PharmacyDetailsDto>(pharmacyModel);
            return pharmacyDto;
        }

        public async Task<PharmacyRegisterDto> PharmacyRegister(PharmacyRegisterDto pharmacyRegisterDto)
        {
            var pharmacyModel = _mapper.Map<Pharmacy>(pharmacyRegisterDto);
            if (pharmacyRegisterDto.Avatar != null)
            {
                pharmacyModel.Avatar = await _fileService.SaveFile(pharmacyRegisterDto.Avatar, ImageConstants.AVATARS_PATH);
            }

            if (pharmacyRegisterDto.Certifications != null)
            {
                pharmacyModel.Certifications = await _fileService.SaveFile(pharmacyRegisterDto.Certifications, ImageConstants.CERTIFICATION_PHARMACIES_PATH);
            }

            await _db.Pharmacies.AddAsync(pharmacyModel);
            await _db.SaveChangesAsync();
            foreach (var awd in pharmacyRegisterDto.Awards)
            {
                awd.PharmacyId = pharmacyModel.Id;
                var awardsModel = _mapper.Map<Awards>(awd);
                await _db.Awards.AddAsync(awardsModel);
            }
            await _db.SaveChangesAsync();
            var pharmacyDto = _mapper.Map<PharmacyRegisterDto>(pharmacyRegisterDto);
            return pharmacyDto;
        }

        public async Task<IEnumerable<PharmacyRequestDetailsDto>> PharmacyRequestList()
        {
            var pharmacyReq = await _db.Pharmacies.Where(d => d.AccountId == null).ToListAsync();
            var pharmacyReqDto = _mapper.Map<IEnumerable<PharmacyRequestDetailsDto>>(pharmacyReq);
            foreach (var pharmacy in pharmacyReqDto)
            {
                var awards = await _db.Awards.Where(d => d.PharmacyId == pharmacy.Id).ToListAsync();
                pharmacy.Awards = _mapper.Map<List<AwardsDto>>(awards);
            }
            return pharmacyReqDto;
        }
    }
}
