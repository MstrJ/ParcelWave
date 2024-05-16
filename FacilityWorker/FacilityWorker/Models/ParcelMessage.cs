namespace FacilityWorker.Models;

public class ParcelMessage
{
    public Identifies? Identifies { get; set; }
    public Attributes? Attributes { get; set; }
    public CurrentState? CurrentState { get; set; }
}

public class Identifies
{
    public string UPID { get; set; }
    public string? Barcode { get; set; }
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
    Ord,Wdr,Dfw,Atl
}