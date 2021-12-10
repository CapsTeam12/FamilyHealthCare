using Contract.Constants;
using Contract.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealCare.UnitTests.TestData
{
    public static class AppointmentTestData
    {
        public static IEnumerable<object[]> ValidDescription()
        {
            return new object[][]
            {
                new object[] {"Re-examination"},
                new object[] {"Examination"},
            };
        }

        public static IEnumerable<object[]> InvalidDescription()
        {
            return new object[][]
            {
                new object[]
                {
                    "    ",
                    string.Format(ErrorMessage.Common.RequiredError, nameof(AppointmentCreateDto.Description))
                },
                new object[]
                {
                    null,
                    string.Format(ErrorMessage.Common.RequiredError, nameof(AppointmentCreateDto.Description))
                },
            };
        }

        public static IEnumerable<object[]> ValidTime()
        {
            return new object[][]
            {
                new object[] { DateTime.Now }
            };
        }

        public static IEnumerable<object[]> InvalidTime()
        {
            return new object[][]
            {
                new object[]
                {
                    DateTime.MinValue,
                    string.Format(ErrorMessage.Common.InvalidTimeValue, nameof(AppointmentCreateDto.Time))
                }
            };
        }
        public static IEnumerable<object[]> ValidTherapistId()
        {
            return new object[][]
            {
                new object[] {1},
                new object[] {2},
            };
        }

        public static IEnumerable<object[]> InvalidTherapistId()
        {
            return new object[][]
            {
                new object[]
                {
                    null,
                    string.Format(ErrorMessage.Common.RequiredError, nameof(AppointmentCreateDto.TherapistId))
                },
            };
        }

        public static Appointment ValidGetAppointment(string search)
        {
            return new Appointment
            {
                Id = new Random().Next(),
                Time = DateTime.Now,
                Description = search,
                TherapistId = new Random().Next(),
            };
        }
        public static AppointmentCreateDto ValidGetAppointmentCreateDto()
        {
            return new AppointmentCreateDto
            {
                Time = DateTime.Now,
                Description = "Re-examination",
                TherapistId = 1,
            };
        }
    }

}
