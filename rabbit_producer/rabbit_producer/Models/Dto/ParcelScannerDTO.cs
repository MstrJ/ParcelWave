using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
namespace rabbit_producer.Models.Dto;

public class ParcelScannerDTO
{
    public string UPID { get; set; }
    
    public float? Width { get; set; }
    
    public float? Length { get; set; }
    
    public float? Depth { get; set; }

    
    public ParcelScannerDTO(string upid, float? width, float? depth, float? length)
    {
        (UPID, Width, Depth, Length) = (upid, width ?? 0, depth ?? 0, length ?? 0);
    }
}