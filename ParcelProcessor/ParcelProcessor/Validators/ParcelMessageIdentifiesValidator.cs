using FluentValidation;
using ParcelProcessor.Communications.Rabbit.Dto;

namespace ParcelProcessor.Validators;

public class ParcelMessageIdentifiesValidator :  AbstractValidator<ParcelMessage>
{
    public ParcelMessageIdentifiesValidator()
    {
        RuleFor(x => x.UPID).NotNull().NotEmpty();
    }
}