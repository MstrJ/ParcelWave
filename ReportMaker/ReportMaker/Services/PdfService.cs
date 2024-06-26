using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using ReportMaker.Services.Interfaces;

namespace ReportMaker.Services;

public class PdfService : IPdfService
{
    public async Task<IActionResult> Create(Func<Document, Task> body)
    {
        var ms = new MemoryStream();
        var pdfName = $"Parcel-Raport-{DateTime.Now:d/M/yy/HH:mm:ss}.pdf";
        var title = $"Parcel Report {DateTime.Now:d/M/yy} {DateTime.Now:h:mm:ss}";
        await using (var writer = new PdfWriter(ms))
        using (var pdfDocument = new PdfDocument(writer))
        using (var document = new Document(pdfDocument))
        {
            writer.SetCloseStream(false);
            document.Add(new Paragraph(title).SetFontSize(36));
            document.Add(new Paragraph("This Pdf file was generated by the ReportMaker").SetFontSize(24));
            await body(document);
        }
        ms.Position = 0;
        
        return new FileContentResult(ms.ToArray(), "application/pdf")
        {
            FileDownloadName = pdfName
        };
    }
}