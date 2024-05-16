using FluentValidation;
using rabbit_producer.Models.Dto;

namespace rabbit_producer.Validations;

public class ParcelScannerValidation : AbstractValidator<ParcelScannerDTO>
{
    public ParcelScannerValidation()
    {
        RuleFor(x => x.Depth).NotNull().GreaterThan(0);
        RuleFor(x => x.Length).NotNull().GreaterThan(0);
        RuleFor(x => x.Width).NotNull().GreaterThan(0);
    }
}