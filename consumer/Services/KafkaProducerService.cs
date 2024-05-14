using com.kuba;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using ParcelEntity = consumer.Models.ParcelEntity;

namespace consumer.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly ProducerConfig _producerConfig = new()
    {
        BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_HOSTNAME") ?? "localhost:9094",
    };
    
    private readonly SchemaRegistryConfig _schemaRegistryConfig = new()
    {
        Url = Environment.GetEnvironmentVariable("SCHEMA_REGISTRY_URL") ?? "http://localhost:8085",
    };
    
    private readonly AvroSerializerConfig _avroSerializerConfig = new()
    {
        BufferBytes = 100
    };

        
    public async Task<bool> ProduceParcel(ParcelEntity parcel)
    {
        try
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);
            using var producer = new ProducerBuilder<string, com.kuba.ParcelEntity>( _producerConfig)
                .SetValueSerializer(new AvroSerializer<com.kuba.ParcelEntity>(schemaRegistry, _avroSerializerConfig))
                .Build();

            var parcelToProduce = new com.kuba.ParcelEntity
            {
                _Id = parcel._Id,
                Identifies = new Identifies
                {
                    UPID = parcel.Identifies.UPID,
                    Barcode = parcel.Identifies.Barcode
                },
                Attributes = new Attributes
                {
                    Depth = parcel.Attributes.Depth ?? -1,
                    Length = parcel.Attributes.Length ?? -1,
                    Weight = parcel.Attributes.Weight ?? -1,
                    Width = parcel.Attributes.Width ?? -1
                },
                CurrentState = new CurrentState
                {
                    Facility = (Facility)Enum.Parse(typeof(Facility), parcel.CurrentState.Facility.ToString())
                }
            };
            await producer.ProduceAsync("parcels", new Message<string, com.kuba.ParcelEntity>
            {
                Key = parcelToProduce.Identifies.UPID,
                Value = parcelToProduce,
            });


            return true;

        }
        catch (Exception e)
        {
            return false;
        }
        
    }
}
