using ParcelProcessor.Communications.Rabbit.Dto;

namespace ParcelProcessor.Services.Interfaces;

public interface IParcelService
{
    Task Process(ParcelMessage receive);
}