using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyHealCare.Test.Assertions;
using FamilyHealCare.Tests.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;


namespace FamilyHealCare.Tests
{
    public static class AssertionExtensions
    {
        public static ActionResultAssertions Should(this ActionResult actualValue)
            => new ActionResultAssertions(actualValue);

        public static ValidationResultAssertions Should(this ValidationResult actualValue)
            => new ValidationResultAssertions(actualValue);
    }
}
