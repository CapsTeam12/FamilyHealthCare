using AppointmentService.Validators;
using Contract.DTOs;
using FamilyHealCare.Tests.Validations;
using FamilyHealCare.UnitTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FamilyHealthCare.UnitTest.Validators
{
    public class AppointmentCreateDtoValidatorShould
    {
        private readonly ValidationTestRunner<AppointmentCreateDtoValidator, AppointmentCreateDto> _testRunner;

        public AppointmentCreateDtoValidatorShould()
        {
            _testRunner = ValidationTestRunner
                .Create<AppointmentCreateDtoValidator, AppointmentCreateDto>(new AppointmentCreateDtoValidator());
        }

        [Theory]
        [MemberData(nameof(AppointmentTestData.ValidDescription), MemberType = typeof(AppointmentTestData))]
        public void NotHaveErrorWhenDescriptionIsValid(string description) =>
            _testRunner
                .For(d => d.Description = description)
                .ShouldNotHaveErrorsFor(e => e.Description);

        [Theory]
        [MemberData(nameof(AppointmentTestData.InvalidDescription), MemberType = typeof(AppointmentTestData))]
        public void HaveErrorWhenDescriptionIsInvalid(string description, string errorMessage) =>
            _testRunner
                .For(d => d.Description = description)
                .ShouldHaveErrorsFor(e => e.Description, errorMessage);

        [Theory]
        [MemberData(nameof(AppointmentTestData.ValidTime), MemberType = typeof(AppointmentTestData))]
        public void NotHaveErrorWhenTimeIsValid(DateTime time) =>
            _testRunner
                .For(d => d.Time = time)
                .ShouldNotHaveErrorsFor(e => e.Time);

        [Theory]
        [MemberData(nameof(AppointmentTestData.InvalidTime), MemberType = typeof(AppointmentTestData))]
        public void HaveErrorWhenTimeIsInvalid(DateTime time, string errorMessage) =>
            _testRunner
                .For(d => d.Time = time)
                .ShouldHaveErrorsFor(e => e.Time, errorMessage);

        [Theory]
        [MemberData(nameof(AppointmentTestData.ValidTherapistId), MemberType = typeof(AppointmentTestData))]
        public void NotHaveErrorWhenTherapistIdIsValid(int therapistId) =>
            _testRunner
                .For(d => d.TherapistId = therapistId)
                .ShouldNotHaveErrorsFor(e => e.TherapistId);

        [Theory]
        [MemberData(nameof(AppointmentTestData.InvalidTherapistId), MemberType = typeof(AppointmentTestData))]
        public void HaveErrorWhenTherapistIdIsInvalid(int therapistId, string errorMessage) =>
            _testRunner
                .For(d => d.TherapistId = therapistId)
                .ShouldHaveErrorsFor(e => e.TherapistId, errorMessage);
    }
}
