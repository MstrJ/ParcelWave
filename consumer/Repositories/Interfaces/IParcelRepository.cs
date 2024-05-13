using consumer.Models;

namespace consumer.Repositories.Interfaces;

public interface IParcelRepository
{
    Task<bool> ParcelAdd(ParcelEntity parcel);
    Task<ParcelEntity> ParcelGet(string upid);
    Task<bool> ParcelUpdate(ParcelEntity parcel);
    Task<bool> ParcelDelete(string upid);
}