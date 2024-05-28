using ParcelProcessor.Communications.Rabbit.Dto;

namespace ParcelProcessor.Services.Interfaces;

public interface IValidatorService
{
    Task<bool> ValidateParcelMessage(ParcelMessage receive);
}
