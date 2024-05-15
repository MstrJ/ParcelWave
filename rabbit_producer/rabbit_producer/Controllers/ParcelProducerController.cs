using Microsoft.AspNetCore.Mvc;
using rabbit_producer.Models.Dto;
using rabbit_producer.Services.Interfaces;

namespace rabbit_producer.Controllers;

[ApiController]
[Route("[controller]")]
public class ParcelProducerController : ControllerBase
{

    private readonly IParcelService _parcelService;
    public ParcelProducerController(IParcelService parcelService)
    {
        _parcelService = parcelService;
    }
    
    [HttpPost("weight")]
    public async Task<IActionResult> Parcel_Weight(ParcelWeightDTO dto)
    {
        var r = await _parcelService.Parcel_Weight(dto);
        if(r) return Ok("Step Parcel weight sent to queue");
        return BadRequest("Step Parcel weight not sent to queue");
    }
    
    [HttpPost("scanner")]
    public async Task<IActionResult> Parcel_Scanner(ParcelScannerDTO dto)
    {
        var r = await _parcelService.Parcel_Scanner(dto);
        if(r) return Ok("Step Parcel scanner sent to queue");
        return BadRequest("StepParcel scanner not sent to queue");
    }
    
    [HttpPost("facility")]
    public async Task<IActionResult> Parcel_Facility(ParcelFacilityDTO dto)
    {
        var r = await _parcelService.Parcel_Facility(dto);
        if(r) return Ok("StepParcel facility sent to queue");
        return BadRequest("Step Parcel facility not sent to queue");
    }
}