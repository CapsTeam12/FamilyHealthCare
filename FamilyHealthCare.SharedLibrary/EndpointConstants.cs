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
            public static string CATEGORIES = "/api/Management/categories";
        }

        public static class AppointmentService
        {
            public static string LIST = "/Appointment/List/";
            public static string DETAILS = "/Appointment/";
            public static string BOOKING = "/Appointment/Booking/";
            public static string RESCHEDULE = "/Appointment/Reschedule/";
        }

        public static class ScheduleService
        {
            public static string CALENDAR = "/Schedule";
            public static string SHIFTS = "/Schedule/Shifts";
            public static string DOCTOR_SCHEDULES = "/Schedule/Doctor";
        }
    }
}
