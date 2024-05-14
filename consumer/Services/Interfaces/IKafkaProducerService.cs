using consumer.Models;

namespace consumer.Services;

public interface IKafkaProducerService
{
    Task<bool> ProduceParcel(ParcelEntity parcel);
}