using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Raporter.Repositories.Interfaces;
using Raporter.Services.Interfaces;

namespace Raporter.Services;

public class PDFService : IPDFService
{

    public IParcelRepository _parcelRepository { get; set; }
    public PDFService(IParcelRepository parcelRepository)
    {
        _parcelRepository = parcelRepository;
    }
    
    public async Task<Task<FileContentResult>> GeneratePDF(string upid)
    {
        // improve structure
        var parcel = await _parcelRepository.Get(upid);
            
        var ms = new MemoryStream();

        await using (var writer = new PdfWriter(ms))
        using (var pdfDocument = new PdfDocument(writer))
        using (var document = new Document(pdfDocument))
        {
            writer.SetCloseStream(false);
            document.Add(new Paragraph($"{parcel.Identifies.UPID}, Current State: {parcel.CurrentState.Facility.ToString()}"));
        }
        ms.Position = 0;

        var pdfName = $"Parcel-Report-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
        return Task.FromResult(new FileContentResult(ms.ToArray(), "application/pdf") { FileDownloadName = pdfName });
    }
}