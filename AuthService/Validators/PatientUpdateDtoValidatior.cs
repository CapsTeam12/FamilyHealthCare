using Business;
using Contract.Constants;
using Contract.DTOs.ManagementService;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Validators
{
    public class PatientUpdateDtoValidatior : BaseValidator<PatientUpdateDto>
    {
        public PatientUpdateDtoValidatior()
        {
            RuleFor(a => a.FullName)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.FullName)));
            RuleFor(a => a.DateOfBirth)
                .Must(IsTimeValid)
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.DateOfBirth)));
            RuleFor(a => a.Gender)
                .Must(IsDataListValueValid)
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Gender)));
            RuleFor(a => a.Languages)
                .Must(IsDataListValueValid)
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Languages)));
            RuleFor(a => a.Address)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Address)));
            RuleFor(a => a.PostalCode)
                .Must(IsDataListValueValid)
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.PostalCode)));
            RuleFor(a => a.Email)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Email)));
            RuleFor(a => a.Phone)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Phone)));

        }

        private bool IsTimeValid(DateTime time)
        {
            return time != DateTime.MinValue;
        }

        private bool IsDataListValueValid(int? value)
        {
            return value != 0;
        }
    }
}
