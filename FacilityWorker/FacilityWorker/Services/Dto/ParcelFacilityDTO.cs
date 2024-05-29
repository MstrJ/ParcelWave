using FacilityWorker.QueueSender.Dto;

namespace FacilityWorker.Services.Dto;

public class ParcelFacilityDTO
{
    public string UPID { get; set; }
    
    public Facility? Facility { get; set; }


    public ParcelFacilityDTO(string upid, Facility? facility)
    {
        (UPID, Facility) = (upid, facility);
    }
}