using Contract.DTOs.MedicineService;
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
        Task<IEnumerable<MedicineDto>> GetMedicinesByPharmacyId(string pharmacyId);
        Task<MedicineDto> CreateMedicine(AddUpdateMedicineDto medicineDto);
        Task<MedicineDto> UpdateMedicine(int id,AddUpdateMedicineDto medicineDto);
        Task<MedicineDto> ReturnMedicine(int id);
        Task<MedicineDto> DeleteMedicine(int id);
    }
}
