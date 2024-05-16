using FluentValidation;
using rabbit_producer.Models.Dto;

namespace rabbit_producer.Validations;

public class ParcelWeightValidation : AbstractValidator<ParcelWeightDTO>
{
    public ParcelWeightValidation()
    {
        RuleFor(x => x.Weight).GreaterThan(0);
    }
}