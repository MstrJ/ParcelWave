using consumer.Models;

namespace consumer.Services;

public interface IParcelService
{
    Task<bool> ParcelCreate(ParcelEntity parcel);
    Task<ParcelEntity> ParcelGet(string upid);
}