using ParcelProcessor.Models;

namespace ParcelProcessor.Services;

public interface IValidatorService
{
    Task<bool> ValidateParcelMessage(ParcelMessage receive);
}
