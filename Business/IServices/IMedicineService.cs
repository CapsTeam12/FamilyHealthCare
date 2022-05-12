using Contract.DTOs.MedicineService;
using Contract.DTOs.SearchService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineDto>> GetMedicines();
        Task<IEnumerable<MedicineDto>> GetMedicinesByPharmacyAccountId(string id);
        Task<IEnumerable<SearchMedicineDto>> GetMedicinesByPharmacyId(int id);
        Task<MedicineDto> CreateMedicine(AddUpdateMedicineDto medicineDto);
        Task<MedicineDto> GetMedicineDetails(int id);
        Task<MedicineDto> UpdateMedicine(int id,AddUpdateMedicineDto medicineDto);
        Task<MedicineDto> ReturnMedicine(int id);
        Task<MedicineDto> DeleteMedicine(int id);
    }
}
