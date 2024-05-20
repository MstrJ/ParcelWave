using ParcelProcessor.Models;
using ParcelProcessor.Repositories.Interfaces;
using Serilog;

namespace ParcelProcessor.Services;

public class ParcelService : IParcelService
{

    private readonly IParcelRepository _parcelRepository;
    private readonly ILogger _logger;
    
    public ParcelService(IParcelRepository parcelRepository,ILogger logger)
    {
        _parcelRepository = parcelRepository;
        _logger = logger;
    }    
    
    public ParcelService(IParcelRepository parcelRepository)
    {
        _parcelRepository = parcelRepository;
    }

    public async Task<bool> Create(ParcelMessage parcel)
    {
        try
        {
            var existingParcel = await _parcelRepository.Get(parcel.Identifies?.UPID!);

            if (existingParcel == null)
            {
                var obj = new ParcelEntity
                {
                    Identifies = new Identifies
                    {
                        Barcode = Guid.NewGuid().ToString(),
                        UPID = parcel.Identifies!.UPID
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
            
            var parcelFromDb = await _parcelRepository.Get(existingParcel.Identifies.UPID);

            if (existingParcel.Equals(parcelFromDb))
            {
                _logger.Information("Parcel [{@parcel}] wasn't changed",parcel.Identifies.UPID);
                return false; // true?>false?
            }
                
            await _parcelRepository.Update(existingParcel);
            
            return true;

        }
        catch (Exception e)
        {
            _logger.Information("Parcel [{@parcel}] is not created",parcel.Identifies.UPID);
            return false;
        }
    }
    
    public async Task<ParcelEntity> Get(string upid)
    {
        return await _parcelRepository.Get(upid);
    }
    
}