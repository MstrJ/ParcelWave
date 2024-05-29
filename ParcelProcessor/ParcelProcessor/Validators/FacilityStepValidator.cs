using FluentValidation;
using ParcelProcessor.Models;

namespace ParcelProcessor.Validators;

public class FacilityStepValidator : AbstractValidator<CurrentState>
{
    public FacilityStepValidator()
    {
        RuleFor(x => x.Facility).IsInEnum();
    }   
}