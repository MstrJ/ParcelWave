using rabbit_producer.Models;

namespace rabbit_producer.QueueSender.Interfaces;

public interface IQueueSender
{
    Task<bool> SendToQueue(ParcelMessage parcel);
}

