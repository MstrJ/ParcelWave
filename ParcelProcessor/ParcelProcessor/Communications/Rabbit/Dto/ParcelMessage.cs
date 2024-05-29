using ParcelProcessor.Models;
namespace ParcelProcessor.Communications.Rabbit.Dto;

public class ParcelMessage : ParcelBaseModel
{
    public string UPID { get; set; }
}
