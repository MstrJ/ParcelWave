using ParcelProcessor.Models;

namespace Raporter.Repositories.Interfaces;

public interface IParcelRepository
{
    Task<ParcelEntity> Get(string upid);
}