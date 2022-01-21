using Business;
using Contract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Contract.Constants;

namespace AuthService.Validators
{
    public class ChangePasswordDtoValidator : BaseValidator<ChangePasswordDtoValidator>
    {
        //public ChangePasswordDtoValidator()
        //{
        //    RuleFor(a => a.StartTime)
        //        .Must(IsTimeValid)
        //        .WithMessage(x => string.Format(ErrorMessage.AppointmentMessage.ErrorTime, nameof(x.StartTime)));
        //    RuleFor(a => a.Description)
        //        .NotEmpty()
        //        .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.Description)));
        //    RuleFor(a => a.TherapistId)
        //        .Must(id => id != 0)
        //        .WithMessage(x => string.Format(ErrorMessage.Common.RequiredError, nameof(x.TherapistId)));

        //}

        //private bool IsCurrentPasswordValid(ChangePasswordDtoValidator model)
        //{
        //    return model.cu
        //}
    }
}
