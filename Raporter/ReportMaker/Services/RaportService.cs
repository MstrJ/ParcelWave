using Microsoft.AspNetCore.Mvc;
using iText.Layout.Element;
using ReportMaker.Repositories.Interfaces;
using ReportMaker.Services.Interfaces;

namespace ReportMaker.Services;

public class RaportService : IRaportService
{
    private IParcelRepository _parcelRepository { get; }
    private IPDFService _pdfService { get;}
    
    public RaportService(IParcelRepository parcelRepository, IPDFService pdfService)
    {
        _parcelRepository = parcelRepository;
        _pdfService = pdfService;
    }
    
    public async Task<IActionResult> GenerateRaport()
    {
        return await _pdfService.Create(async (document) =>
        {
            var parcels = await _parcelRepository.Get();
            foreach (var parcel in parcels)
            {
                document.Add(new AreaBreak());
                document.Add(new Paragraph($"Parcel ID: {parcel._Id}")).SetFontSize(24);
                    
                var info = new List().SetFontSize(16);
                info.Add($"Barcode: {parcel.Identifies?.Barcode}");
                info.Add($"Width: {MessageConverter(parcel.Attributes?.Width)}");
                info.Add($"Weight: {MessageConverter(parcel.Attributes?.Weight)}");
                info.Add($"Length: {MessageConverter(parcel.Attributes?.Length)}");
                info.Add($"Depth: {MessageConverter(parcel.Attributes?.Depth)}");
                info.Add($"Current State: {parcel.CurrentState?.Facility.ToString() ?? "N/A"}");
                
                document.Add(info);
            }
            
        });
    }
    
    private string MessageConverter(float? message)
    {
        return message == null ? "N/A" : message.ToString();
    }
    
}