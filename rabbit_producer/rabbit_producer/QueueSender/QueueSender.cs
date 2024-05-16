using System.Text;
using Newtonsoft.Json;
using rabbit_producer.Models;
using rabbit_producer.QueueSender.Interfaces;
using RabbitMQ.Client;

namespace rabbit_producer.QueueSender;

public class QueueSender : IQueueSender
{
    private readonly IConfiguration _configuration;

    public QueueSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> SendToQueue(ParcelMessage parcel)
    {
        try
        {

            var factory = new ConnectionFactory { HostName = _configuration["RabbitMqHostname"],
                DispatchConsumersAsync = true,
                UserName = _configuration["RabbitMqUsername"],
                Password =_configuration["RabbitMqPassword"]
            };
            using var connection =await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            
            await channel.QueueDeclareAsync(queue: "parcels",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parcel));
            
            await channel.BasicPublishAsync(exchange: String.Empty,
                routingKey: "parcels",
                body: body);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
 

}

