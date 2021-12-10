using Business;
using Contract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Contract.Constants;

namespace AppointmentService.Validators
{
    public class AppointmentCreateDtoValidator : BaseValidator<AppointmentCreateDto>
    {
        public AppointmentCreateDtoValidator()
        {
            RuleFor(a => a.Time)
                .Must(IsTimeValid)
                .WithMessage(x => string.Format(ErrorMessage.Common.InvalidTimeValue, nameof(x.Time)));
            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Description)));
            RuleFor(a => a.TherapistId)
                .Must(id => id > 0)
                .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.TherapistId)));

        }

        private bool IsTimeValid(DateTime time)
        {
            return time != DateTime.MinValue;
        }
    }
}
