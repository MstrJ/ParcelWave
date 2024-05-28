using FluentValidation;
using ParcelProcessor.Repository.Dto;

namespace ParcelProcessor.Validators;

public class ScaleStepValidator : AbstractValidator<Attributes>
{
    public ScaleStepValidator()
    {
        RuleFor(att=>att.Weight).GreaterThan(0).WithMessage("Weight is required");
    }
}