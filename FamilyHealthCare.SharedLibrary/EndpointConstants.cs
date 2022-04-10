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
            public static string PATIENTS = "api/Management/patients";
            public static string SPECIALITIES = "api/Management/specialities";
            public static string PHARMACIES = "api/Management/phamacies";
            public static string DOCTORDETAILS = "api/Management/doctors/{id}";
            public static string CATEGORYSDETAILS = "api/Management/categories";
            public static string PATIENTDETAILS = "api/Management/patients";

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
        }

        public static class AppointmentService
        {
            public static string LIST = "/Appointment/List/";
            public static string DETAILS = "/Appointment/";
            public static string BOOKING = "/Appointment/Booking/";
            public static string RESCHEDULE = "/Appointment/Reschedule/";
            public static string CANCEL = "/Appointment/Cancel/";
        }

        public static class ScheduleService
        {
            public static string CALENDAR = "/Schedule";
            public static string SHIFTS = "/Schedule/Shifts";
            public static string DOCTOR_SCHEDULES = "/Schedule/Doctor";
        }

        public static class NotificationService
        {
            public static string NOTIFICAITON = "/notification";
        }
    }
}
