using FluentValidation;
using ParcelProcessor.Models;

namespace ParcelProcessor.Validators;

public class ScannerStepValidator : AbstractValidator<Attributes>
{
    public ScannerStepValidator()
    {
        RuleFor(att=>att.Width).NotEqual(0).WithMessage("Width is required");
        RuleFor(att=>att.Length).NotEqual(0).WithMessage("Length is required");
        RuleFor(att=>att.Depth).NotEqual(0).WithMessage("Depth is required");
        
    }
}