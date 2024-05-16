using rabbit_producer.Models.Dto;

namespace rabbit_producer.Services.Interfaces;

public interface IParcelService
{
    Task<bool> SendMessage(ParcelWeightDTO dto);
    Task<bool> SendMessage(ParcelScannerDTO dto);
    Task<bool> SendMessage(ParcelFacilityDTO dto);
}