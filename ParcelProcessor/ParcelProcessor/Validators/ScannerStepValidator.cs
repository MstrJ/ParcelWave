using FluentValidation;
using ParcelProcessor.Models;

namespace ParcelProcessor.Validators;

public class ScannerStepValidator : AbstractValidator<Attributes>
{
    public ScannerStepValidator()
    {
        RuleFor(att=>att.Width).GreaterThan(0).WithMessage("Width is required");
        RuleFor(att=>att.Length).GreaterThan(0).WithMessage("Length is required");
        RuleFor(att=>att.Depth).GreaterThan(0).WithMessage("Depth is required");
    }
}