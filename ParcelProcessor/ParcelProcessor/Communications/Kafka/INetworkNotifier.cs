using ParcelProcessor.Repository.Dto;

namespace ParcelProcessor.Communications;

public interface INetworkNotifier
{
    Task<bool> Send(ParcelEntity parcel);
}