using Contract.DTOs.MedicalRecordService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByDoctor(string accountId);
        Task<MedicalRecordDto> CreateMedicalRecord(AddUpdateMedicalRecordDto medicalRecordDto);
        Task<MedicalRecordDto> UpdateMedicalRecord(int id,AddUpdateMedicalRecordDto medicalRecordDto);
        Task<MedicalRecordDto> DeleteMedicalRecord(int id);
    }
}
