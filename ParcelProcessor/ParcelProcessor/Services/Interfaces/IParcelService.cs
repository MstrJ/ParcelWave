using ParcelProcessor.Models;

namespace ParcelProcessor.Services;

public interface IParcelService
{
    Task<bool> Create(ParcelEntity parcel);
    Task<ParcelEntity> Get(string upid);
}