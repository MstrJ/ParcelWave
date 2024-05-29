using FacilityWorker.Services.Dto;
using FluentValidation;

namespace FacilityWorker.Validations;

public class ParcelWeightValidation : AbstractValidator<ParcelWeightDTO>
{
    public ParcelWeightValidation()
    {
        RuleFor(x => x.Weight).GreaterThan(0);
    }
}