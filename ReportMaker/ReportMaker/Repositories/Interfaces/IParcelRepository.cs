using ParcelProcessor.Models;

namespace ReportMaker.Repositories.Interfaces;

public interface IParcelRepository
{
    Task<IEnumerable<ParcelEntity>> Get();
}