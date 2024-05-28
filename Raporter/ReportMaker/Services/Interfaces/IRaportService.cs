using Microsoft.AspNetCore.Mvc;

namespace ReportMaker.Services.Interfaces;

public interface IRaportService                  
{                                                
    Task<IActionResult> GenerateRaport();        
}                                                