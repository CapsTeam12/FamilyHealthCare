using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Constants
{
    public static class ErrorMessage
    {
        public static class Common
        {
            public static string Required = "{0} is required";
        }

        public static class AppointmentMessage
        {
            public static string ErrorTime = "Time is not valid";
        }

        public static class Authentication
        {
            public static string ErrorLogin = "User Name or Password is not correctly. Please try again!";
        }
    }
}
