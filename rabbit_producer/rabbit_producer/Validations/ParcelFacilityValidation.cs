using FluentValidation;
using rabbit_producer.Models.Dto;

namespace rabbit_producer.Validations;

public class ParcelFacilityValidation : AbstractValidator<ParcelFacilityDTO>
{
    public ParcelFacilityValidation()
    {
        RuleFor(x => x.Facility).IsInEnum();
    }
}