using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAppointmentService
    {
        Task<IActionResult> GetAppointmentsAsync(string search);
    }
}
