using System.Text;
using consumer.Enums;
using consumer.Models;
using consumer.Models.Interfaces;
using consumer.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer.Services;

public static class RabbitConsumerService
{
    private static readonly IParcelService _parcelService = Factory.Factory.CreateParcelService();
    private static readonly IKafkaProducerService _kafkaProducerService = Factory.Factory.CreateKafkaProducerService();
    
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
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "kuba",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "bardzotajnehaslo",
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
                        
                    
                    var r_create = await _parcelService.ParcelCreate(json);

                    switch (r_create)
                    {
                        case ParcelCreateEnum.ParcelIsCreated:
                            Console.WriteLine($"Parcel [{json.Identifies.UPID}] is created");

                            await Fee(json);
                            break;
                        case ParcelCreateEnum.ParcelIsUpdated:
                            Console.WriteLine($"Parcel [{json.Identifies.UPID}] is updated");

                            await Fee(json);
                            break;
                        case ParcelCreateEnum.ParcelIsNotChanged:
                            Console.WriteLine($"Parcel [{json.Identifies.UPID}] wasn't changed");
                            break;
                        case ParcelCreateEnum.ParcelIsNotCreated:
                            Console.WriteLine($"Parcel [{json.Identifies.UPID}] is not created");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
              Console.WriteLine("Error connecting to rabbitmq server. Retrying in 5 seconds");
        }

    }


    private static async Task Fee(IParcelEntity json)
    {
        var parcel = await _parcelService.ParcelGet(json.Identifies.UPID);

        if (!parcel.IsReady()) return;
        
        Console.WriteLine($" Parcel [{parcel.Identifies.UPID}] is ready to be shipped! 🚀");
            
        var r_produce = await _kafkaProducerService.ProduceParcel(parcel);
            
        Console.ResetColor();
        if(r_produce) 
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"  Parcel [{parcel.Identifies.UPID}] has been successfully produced to Kafka! 🎉");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Parcel [{parcel.Identifies.UPID}] could not be produced to Kafka. 😞");
        }
        Console.ResetColor();
    }
}