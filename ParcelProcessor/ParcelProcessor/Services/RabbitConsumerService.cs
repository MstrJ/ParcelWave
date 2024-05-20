using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ParcelProcessor.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace ParcelProcessor.Services;

public class RabbitConsumerService : IRabbitConsumerService
{
    private readonly IParcelService _parcelService;
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IValidatorService _validatorService;

    public RabbitConsumerService(IParcelService parcelService, IKafkaProducerService kafkaProducerService,IValidatorService validatorService, ILogger logger, IConfiguration config)
    {
        _validatorService = validatorService;
        _parcelService = parcelService;
        _kafkaProducerService = kafkaProducerService;
        _logger = logger;
        _config = config;
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

                    //validation
                    bool validatorResult = await _validatorService.ValidateParcelMessage(receive);
                    
                    if (!validatorResult) return;    
                    
                    //creating parcel
                    var result = await _parcelService.Create(receive);
                    
                    //check and produce to kafka
                    if (result) await CheckKafkaProduceParcel(receive);
   
                };

                await channel.BasicConsumeAsync(queue: "parcels",
                    autoAck: true,
                    consumer: consumer);
                    
                Task.Delay(200).Wait();
            }
        }
        catch (Exception e)
        {
            _logger.Error("Error connecting to rabbitmq server. Retrying in 5 seconds");
        }
    }


    private  async Task CheckKafkaProduceParcel(ParcelMessage parcelMessage)
    {
        var parcel = await _parcelService.Get(parcelMessage.Identifies?.UPID);

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