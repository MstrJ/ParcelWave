using ParcelProcessor.Models;

namespace ParcelProcessor.Services;

public interface IKafkaProducerService
{
    Task<bool> ProduceParcel(ParcelEntity parcel);
}