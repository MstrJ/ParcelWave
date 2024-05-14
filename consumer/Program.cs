using consumer.Services;
class Program
{
    public static async Task Main()
    {
        while (true)
        {
            await RabbitConsumerService.ConsumeParcel();

            await Task.Delay(5000);
        }
    }
}
