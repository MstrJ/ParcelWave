using FluentValidation;
using FacilityWorker.Models.Dto;

namespace FacilityWorker.Validations;

public class ParcelFacilityValidation : AbstractValidator<ParcelFacilityDTO>
{
    public ParcelFacilityValidation()
    {
        RuleFor(x => x.Facility).IsInEnum();
    }
}