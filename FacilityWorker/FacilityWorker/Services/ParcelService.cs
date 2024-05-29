using FacilityWorker.QueueSender.Dto;
using FacilityWorker.QueueSender.Interfaces;
using FacilityWorker.Services.Dto;
using FacilityWorker.Services.Interfaces;

namespace FacilityWorker.Services;

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
            UPID = dto.UPID,
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
            UPID = dto.UPID,
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
            UPID = dto.UPID,
            CurrentState = new CurrentState
            {
                Facility = dto.Facility
            }
        };
        return await _queueSender.SendToQueue(parcel);
    }
}