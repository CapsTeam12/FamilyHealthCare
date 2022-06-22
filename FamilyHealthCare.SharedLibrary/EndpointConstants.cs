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
            public static string PHARMACIES = "/Management/pharmacies";
            public static string CATEGORIES = "api/Management/categories";
            public static string PATIENTS = "api/Management/patients";
            public static string SPECIALITIES = "/Management/specialities";
            public static string SPECIALIST = "/Management/specialities";
            //public static string PHARMACIES = "api/Management/phamacies";

            public static string DOCTORDETAILS = "api/Management/doctors";
            public static string CATEGORYSDETAILS = "api/Management/categories";
            public static string PATIENTDETAILS = "api/Management/patients";
            public static string PHARMACYDETAILS = "api/Management/pharmacies";

            public static string DOCTORS_REQUESTS = "/Management/DoctorRequestList";
            public static string ACCEPT_DOCTORS_REQUESTS = "/Management/AcceptDoctorRequest";
            public static string DENY_DOCTORS_REQUESTS = "/Management/DenyDoctorRequest";
            public static string DETAILS_DOCTORS_REQUESTS = "/Management/GetDetailsDoctorRequest";
            public static string PHARMACIES_REQUESTS = "/Management/PharmacyRequestList";
            public static string ACCEPT_PHARMACIES_REQUESTS = "/Management/AcceptPharmacyRequest";
            public static string DENY_PHARMACIES_REQUESTS = "/Management/DenyPharmacyRequest";
            public static string DETAILS_PHARMACIES_REQUESTS = "/Management/GetDetailsPharmacyRequest";

            public static string GET_PATIENTS = "/Management/GetTotalPatients";
            public static string GET_DOCTORS = "/Management/GetTotalDoctors";
            public static string GET_PHARMACIES = "/Management/GetTotalPharmacies";
            public static string GET_MEDICALS_DOCTOR = "/Management/GetTotalMedicalsByDoctor";
            public static string GET_MEDICALS_PATIENT = "/Management/GetTotalMedicalsByPatient";
            public static string GET_MEDICINES = "/Management/GetTotalMedicines";
            public static string GET_PRESCRIPTIONS = "/Management/GetTotalPrescriptions";

            public static string ACTIVE_PATIENT = "/Management/active";
            public static string DEACTIVATE_PATIENT = "/Management/deactivate";
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
            public static string PHARMACYSEARCH = "/Search/search/pharmacy";
            public static string DOCTORDETAILS = "/Search/search/doctor";
        }

        public static class AppointmentService
        {
            public static string LIST = "/Appointment/List/";
            public static string DETAILS = "/Appointment/";
            public static string BOOKING = "/Appointment/Booking/";
            public static string RESCHEDULE = "/Appointment/Reschedule/";
            public static string CANCEL = "/Appointment/Cancel?AppointmentId={0}&UserId={1}";
            public static string GET_APPOINTMENTS = "/Appointment/GetTotalAppointments";
            public static string GET_APPOINTMENTS_DOCTOR = "/Appointment/GetTotalAppointmentsByDoctor";
            public static string GET_APPOINTMENTS_PATIENT = "/Appointment/GetTotalAppointmentsByPatient";
        }

        public static class BookingAppointmentService
        {
            public static string BOOKING = "/BookingAppointment/Booking/";
        }

        public static class MedicineService
        {
            public static string MEDICINES = "/Medicine/GetMedicines";
            public static string MEDICINES_PHARMACY = "/Medicine/GetMedicinesByPharmacy";
            public static string MEDICINES_PHARMACY_VIEW = "/Medicine/GetMedicinesByPharmacyId";
            public static string CREATE_MEDICINE = "/Medicine/CreateMedicine";
            public static string DETAILS_MEDICINE = "/Medicine/GetMedicineDetails";
            public static string UPDATE_MEDICINE = "/Medicine/UpdateMedicine";
            public static string RETURN_MEDICINE = "/Medicine/ReturnMedicine";
            public static string DELETE_MEDICINE = "/Medicine/DeleteMedicine";
        }

        public static class MedicalRecordService
        {
            public static string LIST_BY_DOCTOR = "/MedicalRecord/GetMedicalRecords";
            public static string CREATE = "/MedicalRecord/CreateMedicalRecord";
            public static string UPDATE = "/MedicalRecord/UpdateMedicalRecord";
            public static string DELETE = "/MedicalRecord/DeleteMedicalRecord";
            public static string SEND_MEDICAL = "/MedicalRecord/SendMedicalRecord";
        }

        public static class PatientService
        {
            public static string DOCTOR_REGISTER = "/Patient/DoctorRegister";
            public static string PHARMACY_REGISTER = "/Patient/PharmacyRegister";
            public static string CHECK_EMAIL_EXIST = "/Patient/CheckEmailExist";
        }

        public static class PrescriptionService
        {
            public static string LIST_BY_DOCTOR = "/Presciption/GetPrescriptionsByDoctor";
            public static string LIST_BY_PHARMACY = "/Presciption/GetPrescriptionsByPharmacy";
            public static string DETAILS = "/Presciption/GetPrescriptionDetails";
            public static string DETAILS_BY_MEDICAL = "/Presciption/GetPrescriptionDetailsWithMedicalRecord";
            public static string CREATE = "/Presciption/CreatePrescription";
            public static string CREATE_BY_PHARMACY = "/Presciption/CreatePrescriptionByPharmacy";
            public static string UPDATE = "/Presciption/UpdatePrescription";
            public static string UPDATE_BY_PHARMACY = "/Presciption/UpdatePrescriptionByPharmacy";
            public static string DELETE = "/Presciption/DeletePrescription";
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

        public static class NotificationService
        {
            public static string NOTIFICATION = "/notification";
        }
        public static class HealthCheckService
        {
            public static string HEALTHCHECK = "/api/HealthCheck/healthcheck";
            public static string HEALTHCHECKDETAILS = "/api/HealthCheck/HealthCheckDetails";
            public static string HEALTHCHECKLIST = "/api/HealthCheck/HealthCheckList";
            public static string HEALTHCHECKRESULT = "/api/HealthCheck/HealthCheckResult";
        }
    }
}
