using Microsoft.AspNetCore.Mvc;

namespace ReportMaker.Services.Interfaces;

public interface IReportService                  
{                                                
    Task<IActionResult> GenerateRaport();        
}                                                