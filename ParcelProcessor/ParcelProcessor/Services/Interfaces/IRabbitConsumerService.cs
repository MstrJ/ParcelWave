namespace ParcelProcessor.Services;

public interface IRabbitConsumerService
{
    Task ConsumeParcel();
}