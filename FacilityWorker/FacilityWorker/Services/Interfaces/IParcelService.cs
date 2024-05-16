using FacilityWorker.Models.Dto;

namespace FacilityWorker.Services.Interfaces;

public interface IParcelService
{
    Task<bool> SendMessage(ParcelWeightDTO dto);
    Task<bool> SendMessage(ParcelScannerDTO dto);
    Task<bool> SendMessage(ParcelFacilityDTO dto);
}