using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IDashboardService
    {
        int GetTotalPatients();
        int GetTotalDoctors();
        int GetTotalPharmacies();
        int GetTotalMedicalRecordsByDoctor(string id);
        int GetTotalMedicalRecordsByPatient(string id);
        int GetTotalMedicines(string id);
        int GetMedicinesInStock(string id);
        int GetMedicinesOutOfStock(string id);
        int GetMedicinesReturned(string id);
        int GetTotalPrescriptions(string id);
    }
}
