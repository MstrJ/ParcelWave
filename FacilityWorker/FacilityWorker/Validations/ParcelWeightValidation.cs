using FluentValidation;
using FacilityWorker.Models.Dto;

namespace FacilityWorker.Validations;

public class ParcelWeightValidation : AbstractValidator<ParcelWeightDTO>
{
    public ParcelWeightValidation()
    {
        RuleFor(x => x.Weight).GreaterThan(0);
    }
}