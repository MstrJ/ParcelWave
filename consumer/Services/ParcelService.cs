using consumer.Models;
using consumer.Repositories.Interfaces;

namespace consumer.Services;

public class ParcelService : IParcelService
{

    private readonly IParcelRepository _parcelRepository;

    public ParcelService(IParcelRepository parcelRepository)
    {
        _parcelRepository = parcelRepository;
    }



    public async Task<bool> ParcelCreate(ParcelEntity parcel)
    {
        try
        {
            var existingParcel = await _parcelRepository.ParcelGet(parcel.Identifies?.UPID!);

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

                await _parcelRepository.ParcelAdd(obj);
                return true;
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

            await _parcelRepository.ParcelUpdate(existingParcel);
            return true;
                
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    
    public async Task<ParcelEntity> ParcelGet(string upid)
    {
        return await _parcelRepository.ParcelGet(upid);
    }


}