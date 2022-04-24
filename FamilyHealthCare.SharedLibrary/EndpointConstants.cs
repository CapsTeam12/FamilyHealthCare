using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealthCare.SharedLibrary
{
    public static class EndpointConstants
    {
        public static string TEST = "api/values/test";

        public static class ManagementService
        {
            public static string DOCTORS = "/Management/doctors";
            public static string CATEGORIES = "api/Management/categories";
            public static string CATEGORYSDETAILS = "api/Management/categories";
            public static string PATIENTDETAILS = "api/Management/patients";
            public static string DOCTORDETAILS = "api/Management/doctors";
            public static string PHARMACYDETAILS = "api/Management/pharmacies";
            public static string SPECIALIST = "/Management/specialities";
            public static string DOCTORS_REQUESTS = "/Management/DoctorRequestList";
            public static string ACCEPT_DOCTORS_REQUESTS = "/Management/AcceptDoctorRequest";
            public static string DENY_DOCTORS_REQUESTS = "/Management/DenyDoctorRequest";
            public static string DETAILS_DOCTORS_REQUESTS = "/Management/GetDetailsDoctorRequest";
            public static string PHARMACIES_REQUESTS = "/Management/PharmacyRequestList";
            public static string ACCEPT_PHARMACIES_REQUESTS = "/Management/AcceptPharmacyRequest";
            public static string DENY_PHARMACIES_REQUESTS = "/Management/DenyPharmacyRequest";
            public static string DETAILS_PHARMACIES_REQUESTS = "/Management/GetDetailsPharmacyRequest";
        }
        public static class AuthService
        {
            public static string CHANGEPASSWORD = "api/Auth/change-password";
            public static string PATIENTPROFILE = "api/Auth/update-profile";
        }

        public static class SearchService
        {
            public static string SEARCH = "api/Search/search/medicine";
            public static string PATIENTPROFILE = "api/Auth/update-profile";
            public static string DOCTORSEARCH = "/Search/specialist/doctor";
        }

        public static class AppointmentService
        {
            public static string LIST = "/Appointment/List/";
            public static string DETAILS = "/Appointment/";
            public static string BOOKING = "/Appointment/Booking/";
            public static string RESCHEDULE = "/Appointment/Reschedule/";
            public static string CANCEL = "/Appointment/Cancel/";
        }

        public static class BookingAppointmentService
        {
            public static string BOOKING = "/BookingAppointment/Booking/";
        }

        public static class MedicineService
        {
            public static string MEDICINES = "/Medicine/GetMedicines";
            public static string MEDICINES_PHARMACY = "/Medicine/GetMedicinesByPharmacy";
            public static string CREATE_MEDICINE = "/Medicine/CreateMedicine";
            public static string UPDATE_MEDICINE = "/Medicine/UpdateMedicine";
            public static string RETURN_MEDICINE = "/Medicine/ReturnMedicine";
            public static string DELETE_MEDICINE = "/Medicine/DeleteMedicine";
        }

        public static class MedicalRecordService
        {
            public static string LIST_BY_DOCTOR = "/MedicalRecord/GetMedicalRecords";
            public static string CREATE = "/MedicalRecord/CreateMedicalRecord";
        }

        public static class PatientService
        {
            public static string DOCTOR_REGISTER = "/Patient/DoctorRegister";
            public static string PHARMACY_REGISTER = "/Patient/PharmacyRegister";
            public static string CHECK_EMAIL_EXIST = "/Patient/CheckEmailExist";
        }

        public static class ScheduleService
        {
            public static string EVENT_CREATE = "/Schedule";
            public static string CALENDAR = "/Schedule";
            public static string SHIFTS = "/Schedule/Shifts";
            public static string DOCTOR_SCHEDULES = "/Schedule/Doctor";
            public static string MEETING_SCHEDULES = "/Schedule/UpdateScheduleMeeting/";
        }

        public static class ZoomService
        {
            public static string CREATE = "/Zoom/CreateMeeting/";
            public static string CREATE_USER = "/Zoom/CreateUser";
        }
    }
}
