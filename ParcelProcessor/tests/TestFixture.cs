using Microsoft.Extensions.Configuration;
using tests.Helpers;

namespace tests;

public class TestFixture : IDisposable
{
    public static MongoHelper MongoHelper { get; set; }
    public static RabbitProducerHelper RabbitProducerHelper { get; set; }
    public static KafkaConsumeHelper KafkaConsumeHelper { get; set; }
    
    public TestFixture()
    {
        Task.Run(async () => await Program.Main(Array.Empty<string>()));
        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("testappsettings.json")
            .AddEnvironmentVariables()
            .Build();
            
        MongoHelper = new MongoHelper(config);
        RabbitProducerHelper = new RabbitProducerHelper(config);
        KafkaConsumeHelper = new KafkaConsumeHelper(config);
    }

    public async void Dispose()
    {
        await MongoHelper.DeleteAll();
    }
}
