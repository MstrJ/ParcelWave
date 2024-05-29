using FluentValidation;
using FacilityWorker.Services.Dto;

namespace FacilityWorker.Validations;

public class ParcelFacilityValidation : AbstractValidator<ParcelFacilityDTO>
{
    public ParcelFacilityValidation()
    {
        RuleFor(x => x.Facility).IsInEnum();
    }
}