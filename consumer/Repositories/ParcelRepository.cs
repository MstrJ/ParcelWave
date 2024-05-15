using consumer.Models;
using consumer.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace consumer.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly ILogger _logger;
    private readonly IMongoCollection<ParcelEntity> _parcelsCollection;
    private readonly IConfiguration _config;
    
    public ParcelRepository(IMongoClient mongoClient, ILogger logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
        var database = mongoClient.GetDatabase(_config["MongoDatabaseName"]);
        _parcelsCollection = database.GetCollection<ParcelEntity>("ParcelEntities");
        
        InitializeDatabase(database);
    }
    
    private void InitializeDatabase(IMongoDatabase database)
    {
        var filter = new BsonDocument("name", "ParcelEntities");
        var options = new ListCollectionsOptions { Filter = filter };
        var exists = database.ListCollections(options).Any();
            
        if (!exists)
        {
            database.CreateCollection("ParcelEntities");
        }
        
    }
    
    public async Task<bool> Add(ParcelEntity parcel)
    {
        try
        {
            await _parcelsCollection.InsertOneAsync(parcel);
            _logger.Information("Parcel is created \n[{@parcel}]",parcel);
            return true;

        }
        catch (Exception e)
        {
            _logger.Error("Parcel [{@parcel}] is not created",parcel.Identifies.UPID);
            return false;
        }
    }

    public async Task<ParcelEntity> Get(string upid)
    {
        return await _parcelsCollection.Find(p => p.Identifies!.UPID == upid).FirstOrDefaultAsync();
    }
    
    
    public async Task<bool> Update(ParcelEntity parcel)
    {
        try
        {
            await _parcelsCollection.ReplaceOneAsync(p => p.Identifies.UPID == parcel.Identifies.UPID, parcel);
            _logger.Information("Parcel is updated\n[{@parcel}]", parcel);
            return true;

        }
        catch (Exception e)
        {
            _logger.Error("Parcel [{@parcel}] is not updated", parcel.Identifies.UPID);
            return false;
        }
    }
    
    public async Task<bool> Delete(string upid)
    {
        try
        {
            await _parcelsCollection.DeleteOneAsync(p => p.Identifies.UPID == upid);
            _logger.Information("Parcel [{Upid}] is deleted",upid);
            return true;

        }
        catch (Exception e)
        {
            _logger.Error("Parcel [{Upid}] is not deleted",upid);
            return false;
        }
    }
    
}