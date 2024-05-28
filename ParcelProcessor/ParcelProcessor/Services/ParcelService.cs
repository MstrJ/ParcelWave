using ParcelProcessor.Communications.Kafka;
using ParcelProcessor.Communications.Rabbit.Dto;
using ParcelProcessor.Models;
using ParcelProcessor.Repository.Dto;
using ParcelProcessor.Repository.Interfaces;
using ParcelProcessor.Services.Interfaces;
using Serilog;

namespace ParcelProcessor.Services;

public class ParcelService : IParcelService
{
    private readonly IParcelRepository _parcelRepository;
    private readonly ILogger _logger;
    private readonly INetworkNotifier _networkNotifier;
    
    public ParcelService(IParcelRepository parcelRepository,ILogger logger, INetworkNotifier networkNotifier)
    {
        _parcelRepository = parcelRepository;
        _logger = logger;
        _networkNotifier = networkNotifier;
    }    

    private async Task<bool> Upsert(ParcelMessage parcel)
    {
        try
        {
            var existingParcel = await _parcelRepository.Get(parcel.UPID!);

            if (existingParcel == null)
            {
                var obj = new ParcelEntity
                {
                    Identifiers = new Identifiers
                    {
                        Barcode = Guid.NewGuid().ToString(),
                        UPID = parcel.UPID
                    },
                    Attributes = parcel.Attributes ?? new Attributes(),
                    CurrentState = parcel.CurrentState ?? new CurrentState()
                };
                
                return await _parcelRepository.Add(obj);
            }
            
            if (parcel.Attributes != null)
            {
                existingParcel.Attributes.Weight =parcel.Attributes.Weight ?? existingParcel.Attributes.Weight;
                existingParcel.Attributes.Width = parcel.Attributes.Width ?? existingParcel.Attributes.Width;
                existingParcel.Attributes.Depth = parcel.Attributes.Depth ?? existingParcel.Attributes.Depth;
                existingParcel.Attributes.Length = parcel.Attributes.Length ?? existingParcel.Attributes.Length;
            }

            if (parcel.CurrentState != null)
            {
                existingParcel.CurrentState.Facility = parcel.CurrentState.Facility ?? existingParcel.CurrentState.Facility;
            }
            
            var parcelFromDb = await _parcelRepository.Get(existingParcel.Identifiers.UPID);

            if (existingParcel.Equals(parcelFromDb))
            {
                _logger.Information("Parcel [{@parcel}] wasn't changed",parcel.UPID);
                return false; // true?>false?
            }
                
            await _parcelRepository.Update(existingParcel);
            
            return true;

        }
        catch (Exception e)
        {
            _logger.Information("Parcel [{@parcel}] is not created",parcel.UPID);
            return false;
        }
    }
    
    public async Task<ParcelEntity> Get(string upid)
    {
        return await _parcelRepository.Get(upid);
    }
    
    public async Task Process(ParcelMessage receive)
    {
        var result = await Upsert(receive);
        
        if (!result) return;
        
        var parcel = await Get(receive?.UPID);

        if (!parcel.IsReady()) return;
        
        _logger.Information("Parcel [{@parcel}] is ready to be shipped! 🚀",parcel.Identifiers.UPID);

        var networkResult = await _networkNotifier.Send(parcel);
            
        if(networkResult) 
        {
             _logger.Information("Parcel [{@parcel}] has been successfully produced to Kafka! 🎉",parcel.Identifiers.UPID);
            return;
        }
        _logger.Error("Parcel [{@parcel}] could not be produced. 😞",parcel.Identifiers.UPID);
    }
    
}