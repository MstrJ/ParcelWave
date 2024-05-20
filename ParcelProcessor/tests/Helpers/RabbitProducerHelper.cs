using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ParcelProcessor.Models;
using RabbitMQ.Client;

namespace tests.Helpers;

public class RabbitProducerHelper
{
    private ConnectionFactory _factory;

    public RabbitProducerHelper(IConfiguration config)
    {
        _factory = new ConnectionFactory { HostName = config["RabbitMqHostname"],
            DispatchConsumersAsync = true,
            UserName = config["RabbitMqUsername"],
            Password =config["RabbitMqPassword"]
        };
    }
    
    public async Task Send(ParcelMessage parcel)
    {

        using var connection =await _factory.CreateConnectionAsync();
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

    }
}