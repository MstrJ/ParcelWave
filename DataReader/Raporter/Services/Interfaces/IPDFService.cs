using Microsoft.AspNetCore.Mvc;

namespace Raporter.Services.Interfaces;

public interface IPDFService
{
    Task<Task<FileContentResult>> GeneratePDF(string upid);
}