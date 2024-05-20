using FluentValidation;
using ParcelProcessor.Models;

namespace ParcelProcessor.Validators;

public class ParcelMessageIdentifiesValidator :  AbstractValidator<Identifies>
{
    public ParcelMessageIdentifiesValidator()
    {
        RuleFor(x => x.UPID).NotNull().NotEmpty();
    }
}