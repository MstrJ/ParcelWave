using consumer.Models;

namespace consumer.Services;

public interface IParcelService
{
    Task<bool> Create(ParcelEntity parcel);
    Task<ParcelEntity> Get(string upid);
}