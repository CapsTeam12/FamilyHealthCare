using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ISearchService
    {
        public Task<IActionResult> GetSearchMedicineResultAsync(string search);
        public Task<IActionResult> GetDetailsSearchMedicineAsync(int id);
        public Task<IActionResult> GetSearchDoctorResultAsync(string search);
        public Task<IActionResult> GetDetailsSearchDoctorAsync(int id);
        public Task<IActionResult> GetSearchPharmacyResultAsync(string search);
        public Task<IActionResult> GetDetailsSearchPharmacyAsync(int id);
    }
}
