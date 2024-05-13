using consumer.Models;
using consumer.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace consumer.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly IMongoCollection<ParcelEntity> _parcelsCollection;

    public ParcelRepository()
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_CONNECTION") ?? "mongodb://localhost:27017");
        var database = client.GetDatabase("Parcels");
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
    
    public async Task<bool> ParcelAdd(ParcelEntity parcel)
    {
        try
        {
            await _parcelsCollection.InsertOneAsync(parcel);
            return true;

        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<ParcelEntity> ParcelGet(string upid)
    {
        return await _parcelsCollection.Find(p => p.Identifies!.UPID == upid).FirstOrDefaultAsync();
    }
    
    
    public async Task<bool> ParcelUpdate(ParcelEntity parcel)
    {
        try
        {
            await _parcelsCollection.ReplaceOneAsync(p => p.Identifies.UPID == parcel.Identifies.UPID, parcel);
            return true;

        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public async Task<bool> ParcelDelete(string upid)
    {
        try
        {
            await _parcelsCollection.DeleteOneAsync(p => p.Identifies.UPID == upid);
            return true;

        }
        catch (Exception e)
        {
            return false;
        }
    }
    
}