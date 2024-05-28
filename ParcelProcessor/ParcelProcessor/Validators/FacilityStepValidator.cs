using FluentValidation;
using ParcelProcessor.Repository.Dto;

namespace ParcelProcessor.Validators;

public class FacilityStepValidator : AbstractValidator<CurrentState>
{
    public FacilityStepValidator()
    {
        RuleFor(x => x.Facility).IsInEnum();
    }   
}