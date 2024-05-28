namespace ParcelProcessor.Communication.Rabbit;

public interface IRabbitConsumerService
{
    Task ConsumeParcel();
}