using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ParcelProcessor.Models;
using ParcelProcessor.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace ParcelProcessor.Communication.Rabbit;

public class RabbitConsumerService : IRabbitConsumerService
{
    private readonly IParcelService _parcelService;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IValidatorService _validatorService;
        
    public RabbitConsumerService(IParcelService parcelService,IValidatorService validatorService, ILogger logger, IConfiguration config)
    {
        _validatorService = validatorService;
        _parcelService = parcelService;
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
             
            // poprawna , bez while. bez consumerow creating
            
            var consumer = new EventingBasicConsumer(channel);
                
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var receive = JsonConvert.DeserializeObject<ParcelMessage>(json);
                
                _logger.Information("Received Parcel \n{@receive}",receive);
                
                bool validatorResult = await _validatorService.ValidateParcelMessage(receive);
                if (!validatorResult) return;
                    
                await _parcelService.Process();
            };
                
            await channel.BasicConsumeAsync(queue: "parcels",
                autoAck: true,
                consumer: consumer);
        }
        catch (Exception e)
        {
            _logger.Error("Error connecting to rabbitmq server.");
        }
    }
    
    
}