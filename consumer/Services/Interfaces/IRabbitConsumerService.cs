namespace consumer.Services;

public interface IRabbitConsumerService
{
    Task ConsumeParcel();
}