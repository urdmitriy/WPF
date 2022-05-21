using System;
using System.Linq;
using ApiDog.Dto;
using FluentValidation;

namespace DesktopApp.Validation
{
    public class ValidationCustomer : AbstractValidator<CustomerDto>
    {
        public ValidationCustomer()
        {
            var message = "Ошибка в поле {PropertyName}: значение {PropertyValue}";
            RuleFor(x => x.Name)
                .NotNull()
                .MinimumLength(2)
                .WithMessage(message);
            RuleFor(x => x.Description)
                .Must(x => x.All(Char.IsLetter));
        }
    }
}