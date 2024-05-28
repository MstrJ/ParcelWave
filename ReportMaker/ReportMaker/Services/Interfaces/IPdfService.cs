using iText.Layout;
using Microsoft.AspNetCore.Mvc;

namespace ReportMaker.Services.Interfaces;

public interface IPdfService
{
    Task<IActionResult> Create(Func<Document, Task> body);
}