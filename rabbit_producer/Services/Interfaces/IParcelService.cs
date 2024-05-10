using rabbit_producer.Models.Dto;

namespace rabbit_producer.Services.Interfaces;

public interface IParcelService
{
    Task<bool> Parcel_Weight(ParcelWeightDTO dto);
    Task<bool> Parcel_Scanner(ParcelScannerDTO dto);
    Task<bool> Parcel_Facility(ParcelFacilityDTO dto);
}