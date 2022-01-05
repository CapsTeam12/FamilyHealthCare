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
            public static string DOCTORS = "/api/Management/doctors";
            public static string CATEGORIES = "/api/Management/categories";
        }
    }
}
