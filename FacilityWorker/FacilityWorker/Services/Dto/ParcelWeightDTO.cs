namespace FacilityWorker.Services.Dto;

public class ParcelWeightDTO
{
    public string UPID { get; set; }
    
    public float? Weight { get; set; }

    public ParcelWeightDTO(string upid, float? weight)
    {
        (UPID, Weight) = (upid, weight ?? 0);
    }
}