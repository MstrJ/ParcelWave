using FacilityWorker.QueueSender.Dto;

namespace FacilityWorker.QueueSender.Interfaces;

public interface IQueueSender
{
    Task<bool> SendToQueue(ParcelMessage parcel);
}

