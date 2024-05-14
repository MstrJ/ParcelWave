using consumer.Enums;
using consumer.Models;

namespace consumer.Services;

public interface IParcelService
{
    Task<ParcelCreateEnum> ParcelCreate(ParcelEntity parcel);
    Task<ParcelEntity> ParcelGet(string upid);
}