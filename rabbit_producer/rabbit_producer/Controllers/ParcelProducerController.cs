using Microsoft.AspNetCore.Mvc;
using rabbit_producer.Models.Dto;
using rabbit_producer.Services.Interfaces;
using rabbit_producer.Validations;

namespace rabbit_producer.Controllers;

[ApiController]
[Route("[controller]")]
public class ParcelProducerController : ControllerBase
{

    private readonly IParcelService _parcelService;
    private readonly ParcelFacilityValidation _parcelFacilityValidation;
    private readonly ParcelScannerValidation _parcelScannerValidation;
    private readonly ParcelWeightValidation _parcelWeightValidation;
    public ParcelProducerController(IParcelService parcelService, ParcelFacilityValidation parcelFacilityValidation, ParcelScannerValidation parcelScannerValidation,ParcelWeightValidation parcelWeightValidation)
    {
        _parcelService = parcelService;
        _parcelFacilityValidation = parcelFacilityValidation;
        _parcelScannerValidation = parcelScannerValidation;
        _parcelWeightValidation = parcelWeightValidation;
    }
    
    [HttpPost("SendWithWeight")]
    public async Task<IActionResult> SendWithWeight(ParcelWeightDTO dto)
    {
        var validationResult = await _parcelWeightValidation.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var r = await _parcelService.SendMessage(dto);
        if(r) return Ok("Parcel's weight sent to the queue");
        return BadRequest("Parcel's weight not sent to the queue");
    }
    
    [HttpPost("SendWithScannersValues")]
    public async Task<IActionResult> SendWithScannersValues(ParcelScannerDTO dto)
    {
        var validationResult = await _parcelScannerValidation.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var r = await _parcelService.SendMessage(dto);
        if(r) return Ok("Scanner's values sent to the queue");
        return BadRequest("Scanner's values is not sent to the queue");
    }
    
    [HttpPost("SendCurrentState")]
    public async Task<IActionResult> SendCurrentState(ParcelFacilityDTO dto)
    {
        var validationResult = await _parcelFacilityValidation.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var r = await _parcelService.SendMessage(dto);
        if(r) return Ok("Current State sent to the queue");
        return BadRequest("Current State not sent to the queue");
    }
}