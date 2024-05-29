using FluentValidation;
using ParcelProcessor.Models;

namespace ParcelProcessor.Validators;

public class ScaleStepValidator : AbstractValidator<Attributes>
{
    public ScaleStepValidator()
    {
        RuleFor(att=>att.Weight).GreaterThan(0).WithMessage("Weight is required");
    }
}