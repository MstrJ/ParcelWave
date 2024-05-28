using ParcelProcessor.Repository.Dto;

namespace ParcelProcessor.Communications.Rabbit.Dto;

public class ParcelMessage
{
    public string UPID { get; set; }
    public Attributes? Attributes { get; set; }
    public CurrentState? CurrentState { get; set; }
}