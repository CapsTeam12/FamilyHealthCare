using Contract.DTOs.ManagementService;
using Contract.DTOs.PartnerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IParnerService
    {
        Task<DoctorRegisterDto> DoctorRegister(DoctorRegisterDto doctorRegisterDto);
        Task<PharmacyRegisterDto> PharmacyRegister(PharmacyRegisterDto pharmacyRegisterDto);
        Task<IEnumerable<DoctorRequestDetailsDto>> DoctorRequestList();
        Task<IEnumerable<PharmacyRequestDetailsDto>> PharmacyRequestList();
        Task<DoctorDetailsDto> AcceptDoctorRequest(int doctorId);
        Task<PharmacyDetailsDto> AcceptPharmacyRequest(int pharmacyId);
        Task<DoctorDetailsDto> DenyDoctorRequest(int doctorId);
        Task<PharmacyDetailsDto> DenyPharmacyRequest(int pharmacyId);
        Task<DoctorDetailsDto> GetDetailsDoctorRequest(int doctorId);
        Task<PharmacyDetailsDto> GetDetailsPharmacyRequest(int pharmacyId);
        Task<bool> CheckEmailExist(string email);
    }
}
