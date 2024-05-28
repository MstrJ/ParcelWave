namespace ParcelProcessor.Models;

public class ParcelMessage
{
    public string UPID { get; set; }
    public Attributes? Attributes { get; set; }
    public CurrentState? CurrentState { get; set; }
}