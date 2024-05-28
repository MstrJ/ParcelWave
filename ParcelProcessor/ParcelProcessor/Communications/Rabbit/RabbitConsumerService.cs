using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ParcelProcessor.Communications.Rabbit.Dto;
using ParcelProcessor.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace ParcelProcessor.Communications.Rabbit;

public class RabbitConsumerService : BackgroundService
{
    private readonly IParcelService _parcelService;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IValidatorService _validatorService;

    public RabbitConsumerService(IParcelService parcelService, IValidatorService validatorService, ILogger logger,
        IConfiguration config)
    {
        _validatorService = validatorService;
        _parcelService = parcelService;
        _logger = logger;
        _config = config;
    }

    public async Task ConsumeParcel()
    {
        bool isConnected = false;
        while (!isConnected)
        {
            try
            {
                _logger.Information("Consumer is running");
                var hostname = _config["RabbitMqHostname"];

                var factory = new ConnectionFactory
                {
                    HostName = hostname,
                    Port = hostname == "localhost" ? 5672 : -1,
                    UserName = _config["RabbitMqUsername"],
                    Password = _config["RabbitMqPassword"]
                };

                var connection = await factory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "parcels",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _logger.Information("[*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var receive = JsonConvert.DeserializeObject<ParcelMessage>(json);

                    _logger.Information("Received Parcel \n{@receive}", receive);

                    bool validatorResult = await _validatorService.ValidateParcelMessage(receive);
                    if (!validatorResult) return;

                    await _parcelService.Process(receive);
                };

                await channel.BasicConsumeAsync(queue: "parcels",
                    autoAck: false,
                    consumer: consumer);
                isConnected = true;
            }
            catch (Exception e)
            {
                _logger.Error("Error connecting to rabbitmq server.");
                await Task.Delay(5000);
            }
            
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
         await ConsumeParcel();
    }
}