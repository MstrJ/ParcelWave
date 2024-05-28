using ParcelProcessor.Repository.Dto;

namespace ParcelProcessor.Communications.Kafka;

public interface INetworkNotifier
{
    Task<bool> Send(ParcelEntity parcel);
}