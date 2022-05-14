using Business.IServices;
using Data;
using Data.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _db;

        public DashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        public int GetMedicinesInStock(string id)
        {
            var medicines = _db.Medicines.Where(m => m.Status == 1).ToList();
            return medicines.Count;
        }

        public int GetMedicinesOutOfStock(string id)
        {
            var medicines = _db.Medicines.Where(m => m.Status == 1).ToList();
            return medicines.Count;
        }

        public int GetMedicinesReturned(string id)
        {
            var medicines = _db.Medicines.Where(m => m.Status == 1).ToList();
            return medicines.Count;
        }

        public int GetTotalDoctors()
        {
            var doctors = _db.Doctors.ToList();
            return doctors.Count;
        }

        public int GetTotalMedicalRecordsByDoctor(string id)
        {
            var medicals = _db.MedicalRecords.Where(m => m.Doctor.AccountId == id).ToList();
            return medicals.Count;
        }

        public int GetTotalMedicalRecordsByPatient(string id)
        {
            var medicals = _db.MedicalRecords.Where(m => m.Patient.AccountId == id).ToList();
            return medicals.Count;
        }

        public int GetTotalMedicines(string id)
        {
            var medicines = _db.Medicines.ToList();
            return medicines.Count;
        }

        public int GetTotalPatients()
        {
            var patients = _db.Patients.ToList();
            return patients.Count;
        }

        public int GetTotalPharmacies()
        {
            var pharmacies = _db.Pharmacies.ToList();
            return pharmacies.Count;
        }

        public int GetTotalPrescriptions(string id)
        {
            var prescriptions = _db.Prescriptions.Where(p => p.Pharmacy.AccountId == id).ToList();
            return prescriptions.Count;
        }
    }
}
