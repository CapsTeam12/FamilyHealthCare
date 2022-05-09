using Contract.DTOs.PrescriptionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsDoctor(string accountId);
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsPharmacy(string accountId);
        Task<PrescriptionDto> GetPrescriptionDetails(int id);
        Task<PrescriptionDto> AddPrescriptionByDoctor(AddUpdatePrescriptionDto prescriptionDto);
        Task<PrescriptionDto> AddPrescriptionByPharmacy(AddUpdatePrescriptionPharmacyDto prescriptionDto);
        Task<PrescriptionDto> UpdatePrescriptionByDoctor(int id,AddUpdatePrescriptionDto prescriptionDto);
        Task<PrescriptionDto> UpdatePrescriptionByPharmacy(int id, AddUpdatePrescriptionPharmacyDto prescriptionDto);
        Task<PrescriptionDto> DeletePrescription(int id);

    }
}
