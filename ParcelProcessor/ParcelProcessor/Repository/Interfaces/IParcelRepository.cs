using ParcelProcessor.Repository.Dto;

namespace ParcelProcessor.Repository.Interfaces;

public interface IParcelRepository
{
    Task<bool> Add(ParcelEntity parcel);
    Task<ParcelEntity> Get(string upid);
    Task<bool> Update(ParcelEntity parcel);
    Task<bool> Delete(string upid);
    Task<bool> DeleteAll();
}