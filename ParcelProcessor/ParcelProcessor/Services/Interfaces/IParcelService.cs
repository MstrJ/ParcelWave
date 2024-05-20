using ParcelProcessor.Models;

namespace ParcelProcessor.Services;

public interface IParcelService
{
    Task<bool> Create(ParcelMessage parcel);
    Task<ParcelEntity> Get(string upid);
}