namespace ParcelProcessor.Communication.Rabbit;

public interface IRabbitConsumer
{
    Task ConsumeParcel();
}