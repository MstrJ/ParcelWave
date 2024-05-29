using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using FacilityWorker.Models;
using FacilityWorker.QueueSender.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace tests.Helpers;

public class RabbitConsumer
{
    public static async Task<ParcelMessage?> ConsumeLatestMessageAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "kuba",
            Password = "bardzotajnehaslo"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: "parcels",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        
        var result = await channel.BasicGetAsync("parcels", true);

        var message = Encoding.UTF8.GetString(result.Body.ToArray());

        return JsonConvert.DeserializeObject<ParcelMessage>(message);
    }
}