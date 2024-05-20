using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using ParcelEntity = com.kuba.ParcelEntity;

namespace tests.Helpers;

public class KafkaConsumeHelper
{
   private IConfiguration _config;

   public KafkaConsumeHelper(IConfiguration config)
   {
      _config = config;
   }
   
   
   public ParcelEntity ConsumeLatestMessage(int seconds)
   {
      var consumerConfig = new ConsumerConfig
      {
         BootstrapServers = _config["KafkaHostname"],
         GroupId = "consumer",
         AutoOffsetReset = AutoOffsetReset.Earliest
      };

      var schemaRegistryConfig = new SchemaRegistryConfig
      {
         Url = _config["SchemaRegistryUrl"]
      };

      using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
      using var consumer =
         new ConsumerBuilder<string, ParcelEntity>(consumerConfig)
            .SetValueDeserializer(new AvroDeserializer<ParcelEntity>(schemaRegistry).AsSyncOverAsync())
            .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
            .Build();
      consumer.Subscribe("parcels");

      var consumeResult = consumer.Consume(TimeSpan.FromSeconds(seconds));

      if (consumeResult == null) return null;

      consumer.Commit(consumeResult);

      var message = consumeResult.Message.Value;
            
      return message;
   }

}