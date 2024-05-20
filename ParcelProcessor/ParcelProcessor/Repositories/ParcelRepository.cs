using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using ParcelProcessor.Models;
using ParcelProcessor.Repositories.Interfaces;
using Serilog;

namespace ParcelProcessor.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly ILogger _logger;
    private readonly IMongoCollection<ParcelEntity> _parcelsCollection;
    private readonly IConfiguration _config;
    
    
    public ParcelRepository(IMongoClient mongoClient, IConfiguration config)
    {
        _logger = Log.Logger;
        _config = config;
        var database = mongoClient.GetDatabase(_config["MongoDatabaseName"]);
        _parcelsCollection = database.GetCollection<ParcelEntity>("ParcelEntities");
        
        InitializeDatabase(database);
    }
    
    public ParcelRepository(IMongoClient mongoClient, ILogger logger, IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task<bool> DeleteAll()
    {
        try
        {
            await _parcelsCollection.DeleteManyAsync(new BsonDocument());   
            _logger.Information("All parcels are deleted");
            return true;
        }
        catch (Exception e)
        {
            _logger.Error("All parcels are not deleted");
            return false;
        }
    }
}