using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HomeAccounting.DataAccess.Models;
using System.ComponentModel.DataAnnotations;
using Infra.Wpf.Common.Helpers;

namespace HomeAccounting.UI.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            int? length = typeof(Person).GetMaxLength("Name");
            RuleFor(c => c.Name).NotNull().NotEmpty().Matches(@"\S").WithMessage("وارد کردن نام اجباری است.");
            if (length != null)
                RuleFor(c => c.Name).MaximumLength(length.Value).WithMessage($"طول نام بیشتر از {length} می باشد.");
        }
    }
}
