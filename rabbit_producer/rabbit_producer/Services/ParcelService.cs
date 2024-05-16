using rabbit_producer.Models;
using rabbit_producer.Models.Dto;
using rabbit_producer.QueueSender.Interfaces;
using rabbit_producer.Services.Interfaces;

namespace rabbit_producer.Services;

public class ParcelService : IParcelService
{
    
    private readonly IQueueSender _queueSender;

    public ParcelService(IQueueSender queueSender)
    {
        _queueSender = queueSender;
    }
    
    public async Task<bool> SendMessage(ParcelWeightDTO dto)
    {
        var parcel = new ParcelMessage
        {
            Identifies = new Identifies
            {
                UPID = dto.UPID
            },
            Attributes = new Attributes
            {
                Weight = dto.Weight
            }
        };
        return await _queueSender.SendToQueue(parcel);
    }
    
    public async Task<bool> SendMessage(ParcelScannerDTO dto)
    {
        var parcel = new ParcelMessage
        {
            Identifies = new Identifies
            {
                UPID = dto.UPID
            },
            Attributes = new Attributes
            {
                Length = dto.Length,
                Depth = dto.Depth,
                Width = dto.Width
            }
        };
        return await _queueSender.SendToQueue(parcel);
    }
    
    public async Task<bool> SendMessage(ParcelFacilityDTO dto)
    {
        var parcel = new ParcelMessage
        {
            Identifies = new Identifies
            {
                UPID = dto.UPID
            },
            CurrentState = new CurrentState
            {
                Facility = dto.Facility
            }
        };
        return await _queueSender.SendToQueue(parcel);
    }
}