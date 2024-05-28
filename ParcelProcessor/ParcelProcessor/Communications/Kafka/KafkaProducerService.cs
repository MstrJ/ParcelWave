using com.kuba;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using ParcelScheme = com.kuba.ParcelEntity;

namespace ParcelProcessor.Communication.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IConfiguration _config;
    public KafkaProducerService(IConfiguration config)
    {
        _config = config;
    }
        
    public async Task<bool> ProduceParcel(Models.ParcelEntity parcel)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _config["KafkaHostname"]
        };
        var schema = new SchemaRegistryConfig
        {
            Url = _config["SchemaRegistryUrl"]
        };
        
        var avroSerializerConfig = new AvroSerializerConfig
        {
            BufferBytes = 100
        };
            
        
        try
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schema);
            using var producer = new ProducerBuilder<string, ParcelScheme>(config)
                .SetValueSerializer(new AvroSerializer<ParcelScheme>(schemaRegistry, avroSerializerConfig))
                .Build();

            var parcelToProduce = new ParcelScheme
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
