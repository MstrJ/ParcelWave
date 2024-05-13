using System.Text;
using consumer.Models;
using consumer.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer.Services;

public static class RabbitConsumerService
{
    private static readonly IParcelService _parcelService = new ParcelService(new ParcelRepository());
        
    
    public static async Task ConsumeParcel()
    {
        try
        {
            Console.WriteLine("Consumer is running");
            var hostname = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? "localhost";
            var factory = new ConnectionFactory
            {
                HostName =hostname,
                Port= hostname == "localhost" ? 5672 : -1,
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "",
            };
            
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
                
            await channel.QueueDeclareAsync(queue: "parcels",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            Console.WriteLine(" [*] Waiting for messages.");
            while (true)
            {
                var consumer = new EventingBasicConsumer(channel);
                    
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    
                    var json = JsonConvert.DeserializeObject<ParcelEntity>(message);
                    // walidacja
                    // ...
                        
                    
                    var r = await _parcelService.ParcelCreate(json);

                    if(!r) Console.WriteLine("Parcel is not created");
                        
                    var parcel = await _parcelService.ParcelGet(json.Identifies.UPID);
                    
                    
                    if (parcel.IsReady())
                    {
                        Console.WriteLine($"Parcel is ready to be shipped, upid: {parcel.Identifies.UPID}");
                        // kafka
                    }
                };

                await channel.BasicConsumeAsync(queue: "parcels",
                    autoAck: true,
                    consumer: consumer);

                    
                Task.Delay(1000).Wait();
            }
        }
        catch (Exception e)
        {
              // Console.WriteLine(e);
              Console.WriteLine("Error connecting to rabbitmq server. Retrying in 5 seconds");
        }

    } 
}