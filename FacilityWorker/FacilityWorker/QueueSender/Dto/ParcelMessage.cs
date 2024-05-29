namespace FacilityWorker.QueueSender.Dto;

public class ParcelMessage
{
    public string UPID { get; set; }
    public Attributes? Attributes { get; set; }
    public CurrentState? CurrentState { get; set; }
}

public class Attributes
{
    public float? Weight { get; set; }
    public float? Width { get; set; }
    public float? Length { get; set; }
    public float? Depth { get; set; }
}

public class CurrentState
{ 
    public Facility? Facility { get; set; }
}

public enum Facility
{
    Ord,Dfw,Atl
}