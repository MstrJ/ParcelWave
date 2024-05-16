using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using rabbit_producer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace tests.Helpers;

public class RabbitConsumer
{
    public async static Task<ParcelMessage> Consume()
    {

        while (true)
        {
            var parcels = new BlockingCollection<ParcelMessage>();

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = -1,
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

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var parcel = JsonConvert.DeserializeObject<ParcelMessage>(json);

                parcels.Add(parcel);
            };

            await channel.BasicConsumeAsync(queue: "parcels",
                autoAck: true,
                consumer: consumer);

            return parcels.Take();
        }
    }
}