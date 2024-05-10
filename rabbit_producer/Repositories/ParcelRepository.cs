using System.Text;
using Newtonsoft.Json;
using rabbit_producer.Models;
using rabbit_producer.Repositories.Interfaces;
using RabbitMQ.Client;

namespace rabbit_producer.Repositories;

public class ParcelRepository : IParcelRepository
{
    public async Task<bool> Parcel_Weight(ParcelEnity parcel)
    {
        return await SendToQueue(parcel);
    }    
    public async Task<bool> Parcel_Scanner(ParcelEnity parcel)
    {
        return await SendToQueue(parcel);
    }    
    
    public async Task<bool> Parcel_Facility(ParcelEnity parcel)
    {        
        return await SendToQueue(parcel);
    }
    
    private async Task<bool> SendToQueue(ParcelEnity parcel)
    {
        try
        {
            var factory = new ConnectionFactory { HostName = "rabbit",
                DispatchConsumersAsync = true,
                UserName = "kuba",
                Password = "bardzotajnehaslo"
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

