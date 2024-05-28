using Microsoft.AspNetCore.Mvc;
using ReportMaker.Services.Interfaces;

namespace ReportMaker.Controllers;

[ApiController]
[Route("")]
public class ReportMakerController : ControllerBase
{

    private readonly IReportService _reportService;
    public ReportMakerController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    [HttpGet("Report")]
    public async Task<IActionResult> Report()
    {
        var file = await _reportService.GenerateRaport();
            
        return file;
    }
    
}