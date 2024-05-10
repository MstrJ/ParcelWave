using rabbit_producer.Models;

namespace rabbit_producer.Repositories.Interfaces;

public interface IParcelRepository
{
    Task<bool> Parcel_Weight(ParcelEnity parcel);
    Task<bool> Parcel_Scanner(ParcelEnity parcel);
    Task<bool> Parcel_Facility(ParcelEnity parcel);
}