using MongoDB.Driver;
using ParcelProcessor.Models;
using ReportMaker.Repositories.Interfaces;

namespace ReportMaker.Repositories;


public class ParcelRepository : IParcelRepository
{
    private readonly IMongoCollection<ParcelEntity> _parcelsCollection;
    private readonly IConfiguration _config;
        
    public ParcelRepository(IMongoClient mongoClient, IConfiguration config)
    {
        _config = config;
        var database = mongoClient.GetDatabase(_config["MongoDatabaseName"]);
        _parcelsCollection = database.GetCollection<ParcelEntity>(_config["MongoCollectionName"]);
    }
    
    public async Task<IEnumerable<ParcelEntity>> Get() =>
        await _parcelsCollection.Find(_ => true).ToListAsync();
}