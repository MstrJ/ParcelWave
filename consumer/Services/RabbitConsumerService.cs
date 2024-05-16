using System.Text;
using consumer.Models;
using consumer.Models.Interfaces;
using consumer.Validators;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace consumer.Services;

public class RabbitConsumerService : IRabbitConsumerService
{
    private readonly IParcelService _parcelService;
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly ScannerStepValidator _scannerStepValidator;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    public RabbitConsumerService(IParcelService parcelService, IKafkaProducerService kafkaProducerService, ScannerStepValidator scannerStepValidator, ILogger logger, IConfiguration config)
    {
        (_parcelService, _kafkaProducerService, _scannerStepValidator,_logger,_config) =
            (parcelService, kafkaProducerService, scannerStepValidator,logger,config);
    }
    
    public async Task ConsumeParcel()
    {
        try
        {
            _logger.Information("Consumer is running");
            var hostname = _config["RabbitMqHostname"];
            
            var factory = new ConnectionFactory
            {
                HostName =hostname,
                Port= hostname == "localhost" ? 5672 : -1,
                UserName = _config["RabbitMqUsername"],
                Password = _config["RabbitMqPassword"]
            };
            
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
                
            await channel.QueueDeclareAsync(queue: "parcels",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                
             _logger.Information("[*] Waiting for messages.");
            while (true)
            {
                var consumer = new EventingBasicConsumer(channel);
                    
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    
                    var receive = JsonConvert.DeserializeObject<ParcelMessage>(json);
                    
                    _logger.Information("Received Parcel \n{@receive}",receive);
                    
                    // validation
                    if(receive?.Attributes != null && receive.Attributes.ToJson().Length > 0)
                    {
                        var r =  await _scannerStepValidator.ValidateAsync(receive.Attributes);
                            
                        if (!r.IsValid)
                        {
                            foreach (var failure in r.Errors)
                            {
                                _logger.Error("Property {@propertyName} failed validation. Error was: {@errorMessage}",failure.PropertyName,failure.ErrorMessage);
                            }

                            return;
                        }
                    }
                    
                    var parcel = JsonConvert.DeserializeObject<ParcelEntity>(json);

                    //creating parcel
                    var result = await _parcelService.Create(parcel);
                    
                    
                     //check and produce to kafka
                    if (result) await CheckKafkaProduceParcel(parcel); //  Foo
   
                };

                await channel.BasicConsumeAsync(queue: "parcels",
                    autoAck: true,
                    consumer: consumer);
                    
                Task.Delay(500).Wait();
            }
        }
        catch (Exception e)
        {
            _logger.Error("Error connecting to rabbitmq server. Retrying in 5 seconds");
        }
    }


    private  async Task CheckKafkaProduceParcel(IParcelEntity parcelEntity)
    {
        var parcel = await _parcelService.Get(parcelEntity.Identifies.UPID);

        if (!parcel.IsReady()) return;
        
         _logger.Information("Parcel [{@parcel}] is ready to be shipped! 🚀",parcel.Identifies.UPID);
             
        var result = await _kafkaProducerService.ProduceParcel(parcel);
            
        if(result) 
        {
             _logger.Information("Parcel [{@parcel}] has been successfully produced to Kafka! 🎉",parcel.Identifies.UPID);
            return;
        }
        _logger.Error("Parcel [{@parcel}] could not be produced to Kafka. 😞",parcel.Identifies.UPID);
    }
}