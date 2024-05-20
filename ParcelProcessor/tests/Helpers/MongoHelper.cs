using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ParcelProcessor.Models;
using ParcelProcessor.Repositories;
using ParcelProcessor.Repositories.Interfaces;

namespace tests.Helpers;

public class MongoHelper
{
    private IConfiguration _config;
    private IParcelRepository _repository { get; set; }

    public MongoHelper(IConfiguration config)
    {
        _config = config;
        _repository = new ParcelRepository(new MongoClient(_config["MongoUri"]),_config);
    }
    
    public async Task DeleteAll()
    {
        await _repository.DeleteAll();
    }

    public async Task<ParcelEntity> Get(string upid)
    {
        return await _repository.Get(upid);
    }

    public async Task WaitForMongoParcelAndVerify(string upid, Action<ParcelEntity> verifyParcelAction)
    {
        ParcelEntity actual;
        for (var i = 0; i < 10; i++)
        {
            actual = await Get(upid);
            if (actual != null)
            {
                verifyParcelAction(actual);
            }
            
            await Task.Delay(200);
        }
    }
}