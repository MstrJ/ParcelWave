using FluentValidation;
using MongoDB.Bson;
using ParcelProcessor.Models;
using ParcelProcessor.Validators;
using Serilog;
namespace ParcelProcessor.Services;

public class ValidatorService : IValidatorService
{
    private readonly ILogger _logger;
    private readonly ScannerStepValidator _scannerStepValidator;
    private readonly ScaleStepValidator _scaleStepValidator;
    private readonly ParcelMessageIdentifiesValidator _parcelMessageIdentifiesValidator;
    private readonly FacilityStepValidator _facilityStepValidator;
    
    public ValidatorService(ILogger logger, ScannerStepValidator scannerStepValidator, ScaleStepValidator scaleStepValidator, ParcelMessageIdentifiesValidator parcelMessageIdentifiesValidator, FacilityStepValidator facilityStepValidator)
    {
        _logger = logger;
        _scannerStepValidator = scannerStepValidator;
        _scaleStepValidator = scaleStepValidator;
        _parcelMessageIdentifiesValidator = parcelMessageIdentifiesValidator;
        _facilityStepValidator = facilityStepValidator;
    }

    public async Task<bool> ValidateParcelMessage(ParcelMessage receive)
    {
        if (!await ValidateAndLogErrors<Identifies>(_parcelMessageIdentifiesValidator, receive.Identifies))
            return false;
        
        if(receive?.Attributes != null && receive.Attributes.ToJson().Length > 0)
        {
            if (!await ValidateAndLogErrors(_scannerStepValidator, receive.Attributes) ||
                !await ValidateAndLogErrors(_scaleStepValidator, receive.Attributes))
            {
                return false;
            }    
        }
        if(receive.CurrentState !=null && receive.CurrentState.ToJson().Length > 0)
        {
           if (!await ValidateAndLogErrors(_facilityStepValidator, receive.CurrentState))
               return false;
        }
        
        _logger.Information("Receive is valid {@receive}", receive);
        return true;
    }
    
    private async Task<bool> ValidateAndLogErrors<T>(IValidator<T> validator, T item)
    {
        var validationResult = await validator.ValidateAsync(item);
    
        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                _logger.Error("Property {@propertyName} failed validation. Error was: {@errorMessage}", failure.PropertyName, failure.ErrorMessage);
            }
        }
        return validationResult.IsValid;
    }    
}